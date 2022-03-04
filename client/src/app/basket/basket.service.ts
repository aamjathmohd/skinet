import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl=environment.apiUrl;
  private basketSource=new BehaviorSubject<IBasket>(null);  
  basket$=this.basketSource.asObservable();
  private basketTotalSource=new BehaviorSubject<IBasketTotals>(null);
  basketTotal$=this.basketTotalSource.asObservable();

  constructor(private http:HttpClient) { }

  getBasket(id:string){
    return this.http.get(this.baseUrl+'basket?id='+id)
    .pipe(
      map((basket:IBasket)=>{
        this.basketSource.next(basket);
        this.calculateTotal();
      })
    );
  }

  setBasket(basket:IBasket){
    return this.http.post(this.baseUrl+'basket',basket).subscribe((response:IBasket)=>{
      this.basketSource.next(response);
     this.calculateTotal();
    },error=>{
      console.log(error);
    });
  }

  getCurrentBasketValues(){
    return this.basketSource.value;
  }

  addItemToBasket(item:IProduct, quantity=1){
    const itemToadd:IBasketItem=this.mapProductItemtoBasketItem(item, quantity);
    const basket=this.getCurrentBasketValues() ?? this.createBasket();
    basket.items=this.addOrUpdateItems(basket.items,itemToadd,quantity);
    this.setBasket(basket);
  }

  incrementItemQuanity(item:IBasketItem){
    const basket=this.getCurrentBasketValues();
    const foundItemIndex=basket.items.findIndex(x=>x.id===item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item:IBasketItem){
    const basket=this.getCurrentBasketValues();
    const foundItemIndex=basket.items.findIndex(x=>x.id===item.id);
    if(basket.items[foundItemIndex].quantity>1){
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    }
    else{
      this.removeItemFromBasket(item);
    }
  }
  removeItemFromBasket(item: IBasketItem) {
    const basket=this.getCurrentBasketValues();
    if(basket.items.some(x=>x.id===item.id)){
      basket.items=basket.items.filter(i=>i.id!==item.id);
      if(basket.items.length>0){
        this.setBasket(basket);
      }
      else{
        this.deleteBasket(basket);
      }
    }
  }

  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl+'basket?id='+basket.id).subscribe(()=>{
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    },error=>{
      console.log(error);
    })
  }

  private calculateTotal(){
    const basket=this.getCurrentBasketValues();
    const shipping=0;
    const subTotal=basket.items.reduce((a,b)=>(b.price*b.quantity)+a,0);
    const total=subTotal+shipping;
    this.basketTotalSource.next({shipping,total,subTotal});
  }

  private addOrUpdateItems(items: IBasketItem[], itemToadd: IBasketItem, quantity: number): IBasketItem[] {
    console.log(items);
    const index=items.findIndex(i=>i.id===itemToadd.id);
    if(index===-1){
      itemToadd.quantity=quantity;
      items.push(itemToadd);
    }
    else{
      items[index].quantity += quantity;
    }
    return items;
  }
  private createBasket(): IBasket {
    const basket=new Basket();
    localStorage.setItem('basket_id',basket.id);
    return basket;
  }
  private mapProductItemtoBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id:item.id,
      productName:item.name,
      price:item.price,
      pictureUrl:item.pictureUrl,
      quantity,
      brand:item.productBrand,
      type:item.productType
    };
  }

 
}



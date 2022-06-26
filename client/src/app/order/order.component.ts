import { Component, OnInit } from '@angular/core';
import { IOrder } from '../shared/models/order';
import { OrderService } from './order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {
  orders:IOrder[];

  constructor(private orderServices:OrderService) { }

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders(){
    this.orderServices.getOrdersForUser().subscribe((order:IOrder[])=>{
      this.orders=order;
    },error=>{
      console.log(error);
    });
  }

}

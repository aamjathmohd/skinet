import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PaerComponent } from './components/paer/paer.component';
import {CarouselModule} from 'ngx-bootstrap/carousel';


@NgModule({
  declarations: [
    PagingHeaderComponent,
    PaerComponent
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    CarouselModule.forRoot()
    
  ],
  exports:[
    PaginationModule,
    PagingHeaderComponent,
    PaerComponent,
    CarouselModule
  ]
})
export class SharedModule { }

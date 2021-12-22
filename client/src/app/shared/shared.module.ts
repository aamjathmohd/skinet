import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PaerComponent } from './components/paer/paer.component';


@NgModule({
  declarations: [
    PagingHeaderComponent,
    PaerComponent
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot()
  ],
  exports:[
    PaginationModule,
    PagingHeaderComponent,
    PaerComponent
  ]
})
export class SharedModule { }

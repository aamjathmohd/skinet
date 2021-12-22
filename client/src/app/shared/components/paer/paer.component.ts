import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';


@Component({
  selector: 'app-paer',
  templateUrl: './paer.component.html',
  styleUrls: ['./paer.component.scss']
})
export class PaerComponent implements OnInit {
  @Input() totalCount:number;
  @Input() pageSize:number;
  @Output() pageChanged=new EventEmitter<number>();

  constructor() { }

  ngOnInit(): void {
  }

  onPagerChange(event:any){
    this.pageChanged.emit(event.page);
  }

}

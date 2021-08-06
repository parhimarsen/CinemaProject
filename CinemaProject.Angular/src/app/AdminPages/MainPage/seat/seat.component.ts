import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SeatView } from '../../Models/seat';

@Component({
  selector: 'app-seat',
  templateUrl: './seat.component.html',
  styleUrls: ['./seat.component.css'],
})
export class SeatComponent implements OnInit {
  @Input() seat!: SeatView;
  @Output() onSelectSeat = new EventEmitter<any>();

  constructor() {}

  ngOnInit(): void {}

  selectSeat(event: any): void {
    this.onSelectSeat.emit(this.seat);
  }
}

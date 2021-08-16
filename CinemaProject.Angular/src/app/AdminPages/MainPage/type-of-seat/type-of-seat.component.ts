import {
  Component,
  Input,
  OnChanges,
  SimpleChanges,
  Output,
  EventEmitter,
} from '@angular/core';

import { SeatView } from '../../Models/seat';
import { TypeOfSeat } from '../../Models/typeOfSeat';

@Component({
  selector: 'app-type-of-seat',
  templateUrl: './type-of-seat.component.html',
  styleUrls: ['./type-of-seat.component.css'],
})
export class TypeOfSeatComponent implements OnChanges {
  @Input() selectedSeat!: SeatView | null;
  @Input() typesOfSeat!: TypeOfSeat[];
  @Output() onDeleteSeat = new EventEmitter<any>();
  @Output() onUpdateSeat = new EventEmitter<any>();
  typeOfSelectedSeat!: TypeOfSeat;

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    let change = changes['selectedSeat'];
    if (change !== undefined) {
      if (!change.firstChange) {
        this.typeOfSelectedSeat = this.typesOfSeat.find(
          (typeOfSeat) => typeOfSeat.id === this.selectedSeat?.typeOfSeatId
        )!;
        console.log(this.selectedSeat);
      }
    }
  }

  changeTypeOfSeat(event: any) {
    console.log(event.value);
    this.onUpdateSeat.emit(event.value);
  }

  deleteSeat(event: any) {
    this.onDeleteSeat.emit(this.selectedSeat);
  }
}

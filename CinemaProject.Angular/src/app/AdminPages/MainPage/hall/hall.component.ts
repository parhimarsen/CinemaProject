import {
  Component,
  ViewChild,
  ElementRef,
  AfterViewInit,
  ChangeDetectorRef,
} from '@angular/core';

import { forkJoin } from 'rxjs';

import { ActivatedRoute } from '@angular/router';
import { HallsService } from '../../services/halls.service';

import { SeatView } from '../../Models/seat';
import { TypeOfSeat } from '../../Models/typeOfSeat';
import { TypesOfSeatService } from '../../services/types-of-seat.service';
import { SeatsService } from '../../services/seats.service';

@Component({
  selector: 'app-hall',
  templateUrl: './hall.component.html',
  styleUrls: ['./hall.component.css'],
})
export class HallComponent implements AfterViewInit {
  @ViewChild('seatsPerRow')
  seatsPerRow!: ElementRef;

  seats!: SeatView[];
  seatsByRows!: SeatView[][];
  seatsCountPerRow!: number;
  typesOfSeat!: TypeOfSeat[];
  selectedSeat!: SeatView | null;

  row!: number | null;
  cinemaId!: string | null;
  hallId!: string | null;
  label: string = 'Count of Seats';
  maxValue: number = 12;

  constructor(
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private hallsService: HallsService,
    private typesOfSeatService: TypesOfSeatService,
    private seatsService: SeatsService
  ) {
    this.cinemaId = this.route.snapshot.paramMap.get('cinemaId');
    this.hallId = this.route.snapshot.paramMap.get('hallId');
  }

  ngAfterViewInit(): void {
    this.refreshData();
    this.cdr.detectChanges();
  }

  private uuidv4(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(
      /[xy]/g,
      function (c) {
        var r = (Math.random() * 16) | 0,
          v = c == 'x' ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      }
    );
  }

  private getRandomInt(min: number, max: number) {
    return Math.floor(Math.random() * (max - min)) + min;
  }

  private generateRandomColor(): string {
    return (
      'rgb(' +
      this.getRandomInt(0, 255) +
      ', ' +
      this.getRandomInt(0, 255) +
      ', ' +
      this.getRandomInt(0, 255) +
      ')'
    );
  }

  refreshData(): void {
    forkJoin({
      typesOfSeat: this.typesOfSeatService.getAll(),
      seats: this.hallsService.getSeats(this.hallId!),
    }).subscribe(({ typesOfSeat, seats }) => {
      this.typesOfSeat = typesOfSeat.filter(
        (typeOfSeat) => typeOfSeat.cinemaId === this.cinemaId
      );
      this.typesOfSeat.forEach((typeOfSeat) => {
        typeOfSeat.color = this.generateRandomColor();
      });

      this.seats = seats;
      this.seats.forEach((seat) => {
        let typeOfSeat = this.typesOfSeat.find(
          (typeOfSeat) => typeOfSeat.id === seat.typeOfSeatId
        );
        if (typeOfSeat !== undefined) {
          seat.color = typeOfSeat.color;
        }
      });

      this.seatsByRows = [];
      this.seats.forEach((seat) => {
        if (this.seatsByRows[seat.row - 1] === undefined) {
          this.seatsByRows[seat.row - 1] = [];
        }
        this.seatsByRows[seat.row - 1].push(seat);
      });
      this.seatsByRows.reverse().forEach((seatsOfRow) => {
        seatsOfRow.sort((a, b) => a.column - b.column);
      });
      for (let i = 0; i < this.seatsByRows.length; i++) {
        if (typeof this.seatsByRows[i] == 'undefined') {
          this.seatsByRows[i] = [];
        }
      }
    });
    this.typesOfSeatService.getAll();
    this.hallsService.getSeats(this.hallId!);
  }

  updateSeatsCount(value: number): void {
    if (Number.isInteger(value) || value === null) {
      this.seatsCountPerRow = value;
    }
  }

  addNewRow(): void {
    this.seatsByRows.unshift([]);
  }

  changeRow(event: any): void {
    this.row = parseInt(event.value);
  }

  deleteRow(): void {
    let deletedSeats = this.seatsByRows[this.seatsByRows.length - this.row!];
    if (deletedSeats.length !== 0) {
      this.hallsService
        .deleteSeatsRangeRequest(this.hallId!, deletedSeats)
        .subscribe(() => {});
    }

    this.seatsByRows.splice(this.seatsByRows.length - this.row!, 1);
    for (let i = 0; i < this.seatsByRows.length - this.row! + 1; i++) {
      this.seatsByRows[i].forEach((seat) => {
        seat.row--;
      });
      this.hallsService
        .putSeatsRangeRequest(this.hallId!, this.seatsByRows[i])
        .subscribe(() => {});
    }
    this.row = null;
  }

  changeSelectedSeat(seat: any): void {
    if (this.selectedSeat !== seat) {
      this.selectedSeat = seat;
    }
  }

  addSeatesRange(): void {
    if (this.hallId) {
      let existedSeatsCount =
        this.seatsByRows[this.seatsByRows.length - this.row!].length;
      let newSeatsRange: SeatView[] = [];
      let commonTypeOfSeat = this.typesOfSeat.find(
        (typeOfSeat) => typeOfSeat.name === 'Common'
      );
      for (let seatCount = 0; seatCount < this.seatsCountPerRow; seatCount++) {
        let newSeat = new SeatView(
          this.uuidv4(),
          seatCount + 1 + existedSeatsCount,
          this.row!,
          seatCount + 1 + existedSeatsCount,
          this.hallId,
          commonTypeOfSeat!.id,
          commonTypeOfSeat!.color
        );

        newSeatsRange.push(newSeat);
      }
      this.hallsService
        .postSeatsRangeRequest(this.hallId, newSeatsRange)
        .subscribe(() => {
          this.refreshData();
        });
    }
  }

  deleteSelectedSeat(selectedSeat: SeatView): void {
    this.seatsService.deleteSeatRequest(selectedSeat.id).subscribe(() => {
      let deletedSeatIndex = this.seatsByRows[
        this.seatsByRows.length - selectedSeat.row
      ].findIndex((seat) => seat.id === selectedSeat.id);
      this.seatsByRows[this.seatsByRows.length - selectedSeat.row].splice(
        deletedSeatIndex,
        1
      );
      this.selectedSeat = null;
    });
  }

  updateSelectedSeat(newTypeOfSeat: TypeOfSeat): void {
    if (this.selectedSeat !== null) {
      let newSelectedSeat = this.selectedSeat;

      newSelectedSeat.typeOfSeatId = newTypeOfSeat.id;
      newSelectedSeat.color = newTypeOfSeat.color;

      this.seatsService.putSeatRequest(newSelectedSeat).subscribe(() => {
        this.selectedSeat = newSelectedSeat;

        let indexOfSeat = this.seatsByRows[
          this.seatsByRows.length - this.selectedSeat.row
        ].findIndex((seat) => seat.id === this.selectedSeat?.id);
        this.seatsByRows[
          this.seatsByRows.length - this.selectedSeat.row
        ].splice(indexOfSeat, 1);
        this.seatsByRows[
          this.seatsByRows.length - this.selectedSeat.row
        ].splice(indexOfSeat, 0, this.selectedSeat);
      });
    }
  }
}

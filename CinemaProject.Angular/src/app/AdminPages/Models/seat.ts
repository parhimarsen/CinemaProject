import { TypeOfSeat } from './typeOfSeat';

class Seat {
  constructor(
    public id: string,
    public numberOfSeat: number,
    public row: number,
    public column: number,
    public hallId: string,
    public typeOfSeatId: string,
    public color: string
  ) {}
}

class SeatView {
  constructor(
    public id: string,
    public numberOfSeat: number,
    public row: number,
    public column: number,
    public hallId: string,
    public typeOfSeatId: string,
    public color: string
  ) {}
}

export { Seat, SeatView };

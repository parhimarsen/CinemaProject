import { Hall, HallView } from '../Models/hall';
import { TypeOfSeat, TypeOfSeatView } from './typeOfSeat';

class Cinema {
  constructor(
    public id: string,
    public name: string,
    public cityId: string,
    public halls: Hall[] | null,
    public typesOfSeat: TypeOfSeat[] | null
  ) {}
}

class CinemaView {
  constructor(
    private id: string,
    public name: string,
    public halls: HallView[],
    public typesOfSeat: TypeOfSeat[],
    public cityName?: string
  ) {}
}

export { Cinema, CinemaView };

import { Amenity, AmenityView } from './amenity';

class Session {
  constructor(
    public id: string,
    public cost: number,
    public showStart: Date,
    public showEnd: Date,
    public hallId: string,
    public cinemaName: string,
    public hallName: string,
    public filmName: string,
    public filmId: string,
    public affortableAmenities: Amenity[]
  ) {}
}

class SessionView {
  constructor(
    public id: string,
    public cost: string,
    public showStart: string,
    public showEnd: string,
    public cinemaName: string,
    public hallName: string,
    public filmName: string,
    public status: string,
    public affortableAmenities: AmenityView[]
  ) {}
}

export { Session, SessionView };

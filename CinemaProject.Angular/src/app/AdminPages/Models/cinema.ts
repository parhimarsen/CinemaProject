import { City } from './city';

class Cinema {
  constructor(public id: string, public name: string, public cityId: string) {}
}

class CinemaView {
  constructor(
    public id: number,
    public name: string,
    public cityName?: string
  ) {}
}

export { Cinema, CinemaView };

//public Ticket[] Tickets { get; set; }

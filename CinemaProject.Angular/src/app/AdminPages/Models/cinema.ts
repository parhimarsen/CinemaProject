import { Hall, HallView } from '../Models/hall';

class Cinema {
  constructor(
    public id: string,
    public name: string,
    public cityId: string,
    public halls: Hall[] | null
  ) {}
}

class CinemaView {
  constructor(
    public id: number,
    public name: string,
    public halls: HallView[],
    private guidId: string,
    public cityName?: string
  ) {}
}

export { Cinema, CinemaView };

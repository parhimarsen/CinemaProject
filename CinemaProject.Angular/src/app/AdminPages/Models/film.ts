export class Film {
  constructor(
    public id: string,
    public name: string,
    public country: string,
    public releaseDate: Date,
    public duration: string,
    public director: string
  ) {}
}

export class FilmView {
  constructor(
    public id: number,
    public name: string,
    public country: string,
    public releaseDate: string,
    public duration: string,
    public director: string,
    private guidId: string
  ) {}
}

class Hall {
  constructor(
    public id: string,
    public name: string,
    public cinemaId: string
  ) {}
}

class HallView {
  constructor(
    public id: string,
    public name: string,
    public cinemaId: string,
    public cinemaName: string
  ) {}
}

export { Hall, HallView };

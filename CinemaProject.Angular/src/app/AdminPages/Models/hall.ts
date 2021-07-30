class Hall {
  constructor(
    public id: string,
    public name: string,
    public cinemaId: string
  ) {}
}

class HallView {
  constructor(
    public id: number,
    public name: string,
    public cinemaId: string,
    public guidId: string
  ) {}
}

export { Hall, HallView };

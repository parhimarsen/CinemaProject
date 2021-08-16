class TypeOfSeat {
  constructor(
    public id: string,
    public cinemaId: string,
    public name: string,
    public extraPaymentPercent: number,
    public color: string
  ) {}
}

class TypeOfSeatView {
  constructor(
    public id: string,
    public cinemaName: string,
    public name: string,
    public extraPaymentPercent: string,
    public color: string
  ) {}
}

export { TypeOfSeat, TypeOfSeatView };

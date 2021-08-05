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
    public id: number,
    public cinemaName: string,
    public name: string,
    public extraPaymentPercent: string,
    public color: string,
    private guidId: string
  ) {}
}

export { TypeOfSeat, TypeOfSeatView };

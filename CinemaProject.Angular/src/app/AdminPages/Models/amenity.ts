class Amenity {
  constructor(
    public id: string,
    public name: string,
    public cost: number,
    public extraPaymentPercent: number
  ) {}
}

class AmenityView {
  constructor(
    public id: string,
    public name: string,
    public cost: string,
    public extraPaymentPercent: number,
    public sessionId?: string,
    public isChecked: boolean = false
  ) {}
}

export { Amenity, AmenityView };

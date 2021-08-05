class Service {
  constructor(public id: string, public name: string, public cost: number) {}
}

class ServiceView {
  constructor(
    public id: number,
    public name: string,
    public cost: string,
    private guidId: string
  ) {}
}

export { Service, ServiceView };

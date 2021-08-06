class Service {
  constructor(public id: string, public name: string, public cost: number) {}
}

class ServiceView {
  constructor(public id: string, public name: string, public cost: string) {}
}

export { Service, ServiceView };

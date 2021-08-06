class Session {
  constructor(
    public id: string,
    public cost: number,
    public showStart: Date,
    public showEnd: Date,
    public hallId: string,
    public filmId: string
  ) {}
}

class SessionView {
  constructor(
    public id: string,
    public cost: string,
    public showStart: string,
    public showEnd: string,
    public cinemaName: string,
    public hallName: string,
    public filmName: string
  ) {}
}

export { Session, SessionView };

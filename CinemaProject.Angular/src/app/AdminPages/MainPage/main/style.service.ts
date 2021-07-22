import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StyleService {
  isDisplayNone: boolean = true;

  onOpenModal() {
    this.isDisplayNone = false;
  }

  onCloseModal() {
    this.isDisplayNone = true;
  }
}

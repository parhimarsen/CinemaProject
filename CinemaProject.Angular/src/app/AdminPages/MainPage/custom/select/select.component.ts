import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  Input,
} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DefaultEditor } from 'ng2-smart-table';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.css'],
})
export class SelectComponent extends DefaultEditor implements AfterViewInit {
  @Input() value!: any[];
  hallId!: number;

  constructor(
    private router: Router,
    private r: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {
    super();
  }

  ngAfterViewInit(): void {
    this.cdr.detectChanges();
  }

  changeHall(event: any) {
    this.hallId = event.value;
  }

  selectHall(event: any) {
    if (this.value.length !== 0 && this.hallId !== undefined) {
      this.router.navigate([`${this.value[0].cinemaId}/halls/${this.hallId}`], {
        relativeTo: this.r,
      });
    }
  }
}

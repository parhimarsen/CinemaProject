import { Component, OnInit, Input, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-admin-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css'],
})
export class MainComponent implements OnInit {
  service: any;

  constructor(route: ActivatedRoute, injector: Injector) {
    const serviceToken = route.snapshot.data['requiredService'];
    this.service = injector.get(serviceToken);
  }

  ngOnInit(): void {}
}

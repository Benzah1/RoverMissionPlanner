import { Component } from '@angular/core';
import { TimelineComponent } from './components/timeline/timeline';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [TimelineComponent],
  template: `<app-timeline></app-timeline>`,
})
export class App {}

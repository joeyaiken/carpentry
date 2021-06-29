import { Component, Input } from '@angular/core';

@Component({
  selector: 'test-box',
  templateUrl: './test-box.component.html',
  styleUrls: ['./test-box.component.less'],
})
export class TestBoxComponent {
    @Input() color: string;
    @Input() height: number;
    @Input() width: number;
//   title = 'app';
}

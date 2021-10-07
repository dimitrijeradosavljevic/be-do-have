import {Component, Input, OnInit} from '@angular/core';
import {FormControl} from '@angular/forms';

@Component({
  selector: 'app-date-picker-range',
  templateUrl: './date-picker-range.component.html',
  styleUrls: ['./date-picker-range.component.scss'],
})
export class DatePickerRangeComponent implements OnInit {

  @Input() title: string;
  @Input() start: FormControl;
  @Input() end: FormControl;

  constructor() { }

  ngOnInit() {}

}

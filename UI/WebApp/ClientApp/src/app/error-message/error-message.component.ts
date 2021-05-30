import { Component, OnInit } from '@angular/core';
import { ErrorService } from '../services/error/error.service';

@Component({
    selector: 'app-error-message',
    templateUrl: './error-message.component.html',
    styleUrls: ['./error-message.component.css']
})
export class ErrorMessageComponent implements OnInit {
  constructor(public error: ErrorService) {}

    ngOnInit() {}
}

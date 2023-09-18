import { Component } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss'],
})
export class TestComponent {
  constructor(private http: HttpClient) {}

  logInToAllegro() {
    const authUrl = `${environment.allegroLoginEndpoint}`;
    window.location.href = authUrl;
  }
}

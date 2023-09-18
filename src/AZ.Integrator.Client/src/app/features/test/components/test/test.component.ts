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
    // const authUrl = `${environment.allegroApiUrl}?response_type=code&client_id=${environment.allegroApiClientId}&redirect_uri=${environment.allegroApiRedirectUriCallback}`;
    const authUrl = `${environment.allegroLoginEndpoint}`;
    window.location.href = authUrl;

    // @ts-ignore
    // this.http.get(authUrl).subscribe((response: { token: string }) => {
    //   console.warn(response);
    //
    //   // Zapisujemy token w lokalnym magazynie danych.
    //   localStorage.setItem('jwtToken', response.token);
    //
    //   // Możesz teraz użyć tego tokenu do autoryzacji w innych żądaniach HTTP.
    // });
  }
}

import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  
  title = 'DatingApp';
  http = inject(HttpClient);
  users: any;

  ngOnInit(): void {
    this.getUsers(); //Call getUsers method when component initializes
  }

  // Method to fetch users from the backend API
  getUsers() {
    return this.http.get('http://localhost:5124/api/users').subscribe({
      next: (response) => {
        this.users = response;
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log('Request has completed');
      },
    });
  }

  // Constructor with HttpClient can be added here if not using inject (alternative to inject)
  // constructor(private http:HttpClient){}
}

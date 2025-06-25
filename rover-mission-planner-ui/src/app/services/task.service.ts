import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface RoverTask {
  id: string;
  roverName: string;
  taskType: string;
  latitude: number;
  longitude: number;
  startsAt: string;
  durationMinutes: number;
  status: string;
}

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private apiUrl = 'http://localhost:5191';

  constructor(private http: HttpClient) {}

  getTasks(roverName: string, date: string): Observable<RoverTask[]> {
    return this.http.get<RoverTask[]>(
      `${this.apiUrl}/rovers/${roverName}/tasks?date=${date}`
    );
  }
}

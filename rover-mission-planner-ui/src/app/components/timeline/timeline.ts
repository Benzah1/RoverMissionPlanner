import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskService, RoverTask } from '../../services/task.service';

@Component({
  selector: 'app-timeline',
  standalone: true,
  imports: [CommonModule, FormsModule],
  styles: [`
    .container {
      max-width: 800px;
      margin: 2rem auto;
      padding: 1.5rem;
      background: #ffffff;
      border-radius: 8px;
      box-shadow: 0 2px 10px rgba(0,0,0,0.1);
      font-family: 'Segoe UI', sans-serif;
    }

    h2 {
      text-align: center;
      margin-bottom: 1.5rem;
      color: #333;
    }

    .form-group {
      display: flex;
      gap: 1rem;
      margin-bottom: 1rem;
      flex-wrap: wrap;
      justify-content: center;
    }

    input {
      padding: 0.5rem;
      border-radius: 5px;
      border: 1px solid #ccc;
      font-size: 1rem;
      text-transform: uppercase;
    }

    button {
      background-color: #007bff;
      color: white;
      border: none;
      padding: 0.5rem 1rem;
      border-radius: 5px;
      font-size: 1rem;
      cursor: pointer;
    }

    button:hover {
      background-color: #0056b3;
    }

    .timeline-bar {
      position: relative;
      height: 50px;
      background-color: #f1f1f1;
      border-radius: 5px;
      margin: 2rem 0 1rem;
      overflow: visible;
    }

    .task-block {
      position: absolute;
      top: 0;
      height: 100%;
      border-radius: 5px;
      color: white;
      font-size: 12px;
      white-space: nowrap;
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 0 5px;
      cursor: pointer;
    }

    .task-Drill { background-color: #4caf50; }
    .task-Photo { background-color: #2196f3; }
    .task-Sample { background-color: #ff9800; }
    .task-Charge { background-color: #9c27b0; }

    .task-block:hover .tooltip {
      display: block;
    }

    .tooltip {
      display: none;
      position: absolute;
      bottom: 110%;
      left: 50%;
      transform: translateX(-50%);
      background-color: #333;
      color: #fff;
      padding: 8px;
      border-radius: 6px;
      font-size: 12px;
      white-space: pre-line;
      z-index: 100;
      box-shadow: 0 2px 5px rgba(0,0,0,0.2);
    }

    .message {
      text-align: center;
      color: #666;
    }

    .error {
      color: red;
      text-align: center;
    }
  `],
  template: `
    <div class="container">
      <h2>Timeline de tareas</h2>

      <div class="form-group">
        <input [(ngModel)]="roverName" placeholder="Nombre del rover" />
        <input [(ngModel)]="selectedDate" type="date" />
        <button (click)="loadTasks()">Buscar tareas</button>
      </div>

      <div *ngIf="loading" class="message">Cargando...</div>
      <div *ngIf="error" class="error">{{ error }}</div>

      <div class="timeline-bar" *ngIf="tasks.length > 0">
        <div
          *ngFor="let task of tasks"
          class="task-block task-{{task.taskType}}"
          [style.left.%]="getTaskStartPercent(task.startsAt)"
          [style.width.%]="getTaskDurationPercent(task.durationMinutes)"
        >
          {{ task.taskType }}
          <div class="tooltip">
            Tipo: {{ task.taskType }}<br>
            Estado: {{ task.status }}<br>
            Inicio: {{ formatHour(task.startsAt) }}<br>
            Duración: {{ task.durationMinutes }} min<br>
            Lat: {{ task.latitude }}<br>
            Long: {{ task.longitude }}
          </div>
        </div>
      </div>

      <div *ngIf="!loading && tasks.length === 0" class="message">
        No hay tareas para este día.
      </div>
    </div>
  `
})
export class TimelineComponent {
  roverName = 'spirit';
  selectedDate = new Date().toISOString().split('T')[0];
  tasks: RoverTask[] = [];
  loading = false;
  error: string | null = null;

  constructor(private taskService: TaskService) {}

  loadTasks() {
    this.loading = true;
    this.error = null;

    this.taskService.getTasks(this.roverName.toUpperCase(), this.selectedDate).subscribe({
      next: (data) => {
        this.tasks = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Error al cargar las tareas.';
        this.loading = false;
      }
    });
  }

  getTaskStartPercent(startsAt: string): number {
    const date = new Date(startsAt);
    return ((date.getHours() * 60 + date.getMinutes()) / 1440) * 100;
  }

  getTaskDurationPercent(duration: number): number {
    return ((duration / 1440) * 100) * 1.4;
  }

  formatHour(datetime: string): string {
    const date = new Date(datetime);
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }
}

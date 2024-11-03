import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { KeycloakService } from 'keycloak-angular';
import { DbService } from '../service/db-service.service';

@Component({
  selector: 'app-assignment-overview',
  templateUrl: './assignment-overview.component.html',
  styleUrls: ['./assignment-overview.component.css']
})
export class AssignmentOverviewComponent implements OnInit {
  firstName: string | null = '';
  ifUserName: string | null = '';
  assignments: any[] = []; // Array to hold parsed assignment data

  constructor(
    private keycloakService: KeycloakService,
    private router: Router,
    private restDb: DbService
  ) {}

  ngOnInit(): void {
    this.firstName = sessionStorage.getItem('firstName');
    this.ifUserName = sessionStorage.getItem('ifUserName') || '';

    // Fetch assignments from the service
    this.restDb.GetAssignmentsByUsernameForTeacher(this.ifUserName).subscribe((data: any) => {
      // Parse the assignments and students
      if (data && data.$values) {
        this.assignments = data.$values.map((assignment: { assignmentName: any; dueDate: any; exerciseName: any; teacher: { firstname: any; lastname: any; }; students: { $values: any[]; }; }) => ({
          assignmentName: assignment.assignmentName,
          dueDate: assignment.dueDate,
          exerciseName: assignment.exerciseName,
          teacher: `${assignment.teacher.firstname} ${assignment.teacher.lastname}`,
          students: assignment.students.$values.map(student => ({
            studentFirstname: student.firstname,
            studentLastname: student.lastname,
            studentUsername: student.username
          }))
        }));
      }
      console.log(this.assignments); // Log the parsed assignments
    });
  }

  async logout(): Promise<void> {
    sessionStorage.setItem('shouldLogOut', 'true');
    this.router.navigate(['/login']);
  }
}
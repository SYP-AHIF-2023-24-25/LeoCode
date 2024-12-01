import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
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
  selectedAssignment: any = null; // To hold the selected assignment details

  constructor(
    private keycloakService: KeycloakService,
    private router: Router,
    private restDb: DbService
  ) {
    
  }

  ngOnInit(): void {
    this.firstName = sessionStorage.getItem('firstName');
    this.ifUserName = sessionStorage.getItem('ifUserName') || '';

    // Fetch assignments from the service
    this.restDb.GetAssignmentsByUsernameForTeacher(this.ifUserName).subscribe((data: any) => {
      // Parse the assignments and students
      console.log(data);
      if (data) {
        this.assignments = data.map((assignment: { 
          assignmentName: any; 
          dueDate: any; 
          exerciseName: any; 
          teacher: { firstname: any; lastname: any; }; 
          students: any[];
          exercise: { tags: any[]; language: string; exerciseName: string}; // exercise contains tags and language
        }) => ({
          assignmentName: assignment.assignmentName,
          dueDate: assignment.dueDate,
          exerciseName: assignment.exercise?.exerciseName,
          teacher: `${assignment.teacher.firstname} ${assignment.teacher.lastname}`,
          students: assignment.students.map(student => ({
            studentFirstname: student.firstname,
            studentLastname: student.lastname,
            studentUsername: student.username,
            studentTotalTests: student.totalTests || 0,
            studentPassedTests: student.passedTests || 0, // Falls null oder undefined, auf 0 setzen
    studentFailedTests: student.failedTests || 0, // Falls null oder undefined, auf 0 setzen
    studentPercentage: student.totalTests 
        ? Math.round((student.passedTests / student.totalTests) * 100) 
        : 0 // Falls totalTests null oder 0 ist, auf 0 setzen
          })),
          tags: assignment.exercise?.tags || [],  // Handle possible null or undefined
          language: assignment.exercise?.language || 'No language specified',  // Default value if no language is provided
          showStudents: false // Initially hide the students
        }));
      }
      console.log(this.assignments); // Log the parsed assignments
    });
  }

  // Select the assignment to show details
  selectAssignment(assignment: any): void {
    this.selectedAssignment = assignment;
  }


  async logout(): Promise<void> {
    sessionStorage.setItem('shouldLogOut', 'true');
    this.router.navigate(['/login']);
  }

  // Method to toggle the visibility of students list
  toggleStudents(assignment: any): void {
    assignment.showStudents = !assignment.showStudents;
  }

  
}

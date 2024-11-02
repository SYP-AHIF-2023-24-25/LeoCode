import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DbService } from '../service/db-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-assignment',
  templateUrl: './create-assignment.component.html',
  styleUrls: ['./create-assignment.component.css']
})
export class CreateAssignmentComponent implements OnInit {
  assignmentForm: FormGroup;
  assignmentData: any;

  constructor(
    private fb: FormBuilder,
    private dbRest: DbService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {
    this.assignmentForm = this.fb.group({
      exerciseName: [{ value: '', disabled: true }, Validators.required],
      creator: [{ value: '', disabled: true }, Validators.required],
      dateDue: ['', Validators.required],
      name: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.assignmentData = history.state.assignmentData;

    if (this.assignmentData) {
      this.assignmentForm.patchValue({
        exerciseName: this.assignmentData.exerciseName,
        creator: this.assignmentData.creator,
        dateDue: this.assignmentData.dateDue,
        name: this.assignmentData.name
      });
    }
  }

  onSubmit(): void {
    if (this.assignmentForm.valid) {
      const { dateDue, name } = this.assignmentForm.value;
      this.dbRest.AddAssignment(this.assignmentData.exerciseName, this.assignmentData.creator, dateDue, name).subscribe({
        next: (link: string) => { // Expecting a string response (the assignment link)
            console.log('Assignment link:', link);
            this.snackBar.open('Assignment successfully created', 'Close', {
                duration: 5000,
                horizontalPosition: 'center',
                verticalPosition: 'top',
            });
            // Handle the link as needed
        },
        error: (err) => {
            console.error('Error creating assignment:', err);
            this.snackBar.open('Failed to create assignment', 'Close', {
                duration: 5000,
                horizontalPosition: 'center',
                verticalPosition: 'top',
            });
        }
    });
    } else {
      this.snackBar.open('Please fill out all fields correctly.', 'Close', {
        duration: 5000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
      });
    }
  }
}

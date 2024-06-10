import { ElementCompact, xml2js } from 'xml-js';
import { Component, OnInit } from '@angular/core';
import {Tags} from '../model/tags.enum';
import { Exercise } from '../model/exercise';
import * as JSZip from 'jszip';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject } from '@angular/core';
import { RestService } from '../service/rest.service';
import { FileUploadService } from '../service/file-upload-service.service';
import {MatChipsModule} from '@angular/material/chips';
import { FormControl } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { Observable } from 'rxjs';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { startWith } from 'rxjs/operators';
import { map } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { DbService } from '../service/db-service.service';
import { CodeSection } from '../model/code-sections';
import { ArrayOfSnippetsDto } from '../model/arrayOfSnippetsDto';


@Component({
  selector: 'app-create-exercise',
  templateUrl: './create-exercise.component.html',
  styleUrls: ['./create-exercise.component.css']
})
export class CreateExerciseComponent {

  constructor(private fileUploadService: FileUploadService, private rest: RestService, private dbRest: DbService, private http: HttpClient, private snackBar: MatSnackBar, private router: Router) {
    this.filteredTags = this.tagCtrl.valueChanges.pipe(
      startWith(null),
      map((tag: string | null) => tag ? this._filter(tag) : this.availableTags.slice()));  
    
   }
  
  exerciseName: string = ''; 
  currentStep: number = 1;
  instruction: string = '';
  selectedLanguage: string = '';
 
  zipFile: File | null = null;
  emptyZipFile: File | null = null;
  emptyZipFileUploaded: boolean = false;
  ZipFileUploaded: boolean = false;
  isUploading = false;


  // property für die ausgewählten Tags
  tagCtrl = new FormControl();
  selectedTags: string[] = [];  
  availableTags: string[] = Object.values(Tags); // ersetzen Sie dies durch Ihre tatsächlichen Tags
  filteredTags: Observable<string[]> | undefined;  
  separatorKeysCodes: number[] = [13, 188]; // Enter und Komma  

  ngOnInit(): void {
    document.addEventListener("DOMContentLoaded", () => {
      const mainMenuLinks = document.querySelectorAll('.main-menu-link');
      
      mainMenuLinks.forEach(link => {
        link.addEventListener('click', (event) => {
          event.preventDefault();
          const subMenu = link.nextElementSibling;
          if (subMenu) {
            subMenu.classList.toggle('show');
          }
        });
      });
    });
  }


  // navigaton zwischen den schritten
  nextStep() {
    this.currentStep++;
  }

  goToStep(step: number) {
    this.currentStep = step;
  }


  addInstructions(instructions: string) {
    this.instruction = instructions;
    this.nextStep(); 
  }

  selectLanguage(language: string) {
    this.selectedLanguage = language;
    this.nextStep(); 
  }

  // tags auwählen
  toggleTagSelection(tag: string) {
    if (this.selectedTags.includes(tag)) {
      this.selectedTags = this.selectedTags.filter(t => t !== tag);
    } else {
      this.selectedTags.push(tag);
    }
  }

  addTag(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    // Add our tag
    if ((value || '').trim()) {
      this.selectedTags.push(value.trim().toUpperCase());
      this.availableTags.push(value.trim().toUpperCase());
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }

    this.tagCtrl.setValue(null);
  }


  isSelectedTag(tag: string): boolean {
    return this.selectedTags.includes(tag);
  }

  removeTag(tag: string): void {
    const index = this.selectedTags.indexOf(tag);

    if (index >= 0) {
      this.selectedTags.splice(index, 1);
    }
  }


  selected(event: MatAutocompleteSelectedEvent): void {
    this.selectedTags.push(event.option.viewValue);
    this.tagCtrl.setValue(null);
  }



  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.availableTags.filter(tag => tag.toLowerCase().includes(filterValue));
  }

  // Upload Zip File (Step 4)
  uploadEmptyZipFile(event: DragEvent) {
    const file = event.dataTransfer;

    if (file && file.files && file.files.length > 0) {
      this.emptyZipFile = file.files[0];
      this.emptyZipFileUploaded = true;
    } else {

      console.error('No file selected');
      this.snackBar.open('No file selected', 'Close', {
        duration: 5000,
      });
    }
  }

  uploadZipFile(event: DragEvent) {
    const file = event.dataTransfer;

    if (file && file.files && file.files.length > 0) {
      this.zipFile = file.files[0];
      this.ZipFileUploaded = true;
    } else {
      console.error('No file selected');
      this.snackBar.open('No file selected', 'Close', {
        duration: 5000,
      });
    }
  }

  async uploadZipFileToCSharpRunner(file: File, content: string): Promise<any> {
    if (!file) {
      console.error('No file selected');
      return; // Return early if no file is selected
    }
    this.isUploading = true;
    try {
      let result = await (await this.fileUploadService.uploadCSharpTemplate(file, content)).toPromise();
      console.log(result);
      this.isUploading = false;
      return result;
    } catch (error) {
      console.error('Error occurred during file upload:', error);
      this.isUploading = false;
      throw error; // Rethrow the error for handling in the calling function
    }
  }

  async uploadZipToTsRunner(file: File, content: string): Promise<void > {
    if (!file) {
      console.error('No file selected');
      this.snackBar.open('No file selected', 'Close', {
        duration: 5000,
      });
      return; // Return early if no file is selected
    }

    this.isUploading = true;
  
    try {
      const response: any = await this.fileUploadService.uploadFile(file, content).toPromise();
      console.log(response);
      this.isUploading = false;
      return response;
    } catch (error) {
      console.error('Error occurred during file upload:', error);
      this.snackBar.open('Error occurred during file upload: ' + (error as any).message, 'Close', {
        duration: 5000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
      });
      this.isUploading = false;
      throw error; // Rethrow the error for handling in the calling function
    }
  
  }
  

  async sendCodeToRunner() {
     let exercise  = {
      name: this.exerciseName,
      instruction: this.instruction,
      language: this.selectedLanguage,
      tags: this.selectedTags,
      zipFile: this.zipFile,
      emptyZipFile: this.emptyZipFile
    };

    if (exercise.zipFile) {
      if(exercise.language === 'TypeScript'){
        const fullResponse = await this.uploadZipToTsRunner(exercise.zipFile, "full");
        if (exercise.emptyZipFile && this.testsMatchPasses(fullResponse)) {
          await this.uploadZipToTsRunner(exercise.emptyZipFile, "empty");
          let name: string[] = exercise.emptyZipFile.name.split('.');
          console.log(name[0]);
          const response: any = await this.rest.getCode(name[0]).toPromise();
          console.log(response);
          let codeSections: CodeSection[] = this.parseTemplateToCodeSections(response, name[0]);
          console.log(codeSections);

          let arrayOfSnippets : ArrayOfSnippetsDto = {
            snippets: codeSections
          }
          
          console.log(arrayOfSnippets);
            

          this.dbRest.AddExercise(arrayOfSnippets, exercise.name, exercise.instruction, exercise.language, exercise.tags, "Default").subscribe((data: Exercise) => {
            console.log(data);
          });

        }
      }
      else if(exercise.language === 'Csharp'){
        const fullResponse = await this.uploadZipFileToCSharpRunner(exercise.zipFile, "full");

        if (this.testsMatchPassesCSharp(fullResponse) && exercise.emptyZipFile) {
          await this.uploadZipFileToCSharpRunner(exercise.emptyZipFile, "empty");
          let name: string[] = exercise.emptyZipFile.name.split('.');
          console.log(name[0]);
          const response: any = await this.rest.getCodeCSharp(name[0]).toPromise();
          console.log(response);
          let codeSections: CodeSection[] = this.parseTemplateToCodeSections(response.value, name[0]);
          console.log(codeSections);

          let arrayOfSnippets : ArrayOfSnippetsDto = {
            snippets: codeSections
          }
          
          console.log(arrayOfSnippets);
            

          this.dbRest.AddExercise(arrayOfSnippets, exercise.name, exercise.instruction, exercise.language, exercise.tags, "Default").subscribe((data: Exercise) => {
            console.log(data);
          });
        }
      }
      this.snackBar.open('Exercise created successfully', 'Close', {
        duration: 5000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
      });

      this.router.navigate(['/start-screen']).then(() => {
        window.location.reload();
      });
      
    }else{
      console.error('No file selected');
      this.snackBar.open('No ZIP file !!!', 'Close', {
        duration: 5000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
      });
    }
  }
  testsMatchPassesCSharp(response: any): boolean {
    const xmlString = response;
    const xmlObject = xml2js(xmlString, { compact: true }) as ElementCompact; // Add type assertion
    const totalTests = xmlObject['TestRun'].ResultSummary.Counters._attributes.total;
    const passedTests = xmlObject['TestRun'].ResultSummary.Counters._attributes.passed;
    return totalTests === passedTests;
  }

  testsMatchPasses(response: any): boolean {
    return response && response.tests && response.passes &&
           response.tests.length === response.passes.length;
  }

  sleep(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
  resetForm() {
    this.currentStep = 1;
    this.instruction = '';
    this.selectedLanguage = '';
    this.selectedTags = [];
    this.zipFile = null;
    this.emptyZipFile = null;
  }

  clearEmptyZipFile() {
    this.emptyZipFile = null;
    this.emptyZipFileUploaded = false;
    console.log(this.emptyZipFile);
  }

  clearZipFile() {
    this.zipFile = null;
    this.ZipFileUploaded = false;
    console.log(this.zipFile);
  }

  parseTemplateToCodeSections(template: string, programName: string) :CodeSection[]{

    let stringCodeSections: string[]= template.split("\n");

    let codeSections: CodeSection[] = [];


    for(let i = 0; i < stringCodeSections.length; i++) {
      if(stringCodeSections[i].includes("Todo") || stringCodeSections[i].includes("throw new NotImplementedException()")) {
        codeSections.push({ code: stringCodeSections[i], readOnlySection: false, fileName: programName});
      }else {
        codeSections.push({ code: stringCodeSections[i], readOnlySection: true, fileName: programName});
      }
    }
    return codeSections;
  }

}



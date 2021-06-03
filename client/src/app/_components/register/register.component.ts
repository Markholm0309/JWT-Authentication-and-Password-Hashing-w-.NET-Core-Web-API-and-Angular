import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() usersFromHomeComponent: any;
  @Output() cancelRegiser = new EventEmitter();
  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: String[] = [];

  constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initilalizeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initilalizeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    })
  }

  matchValues(macthTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[macthTo].value ? null : {isMatching: true}
    }
  }

  register() {
    this.accountService.regiser(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/');
    }, error => {
      this.validationErrors = error;
    })
  }

  cancel() {
    this.cancelRegiser.emit(false);
  }
}

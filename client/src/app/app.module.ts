import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './_components/home/home.component';
import { RegisterComponent } from './_components/register/register.component';
import { SharedModule } from './_modules/shared.module';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { MatSliderModule } from '@angular/material/slider';
import { TextInputsComponent } from './_components/_forms/text-inputs/text-inputs.component';
import { DateInputComponent } from './_components/_forms/date-input/date-input.component';
import { NavComponent } from './_components/nav/nav.component';
import { LandingpageComponent } from './_components/landingpage/landingpage.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    TextInputsComponent,
    DateInputComponent,
    LandingpageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    SharedModule,
    MatSliderModule,
    ReactiveFormsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

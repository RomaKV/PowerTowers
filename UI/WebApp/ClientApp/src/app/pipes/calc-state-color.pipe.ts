import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'CalcStateColor'
})
export class CalcStateColorPipe implements PipeTransform {
  transform(value: number, min: number, max: number): string {

    if ((Number(value) > Number(max)) || (Number(value) < Number(min))) {

      return 'red';
   
    } else {

      return 'green';
    }

  }
}


    


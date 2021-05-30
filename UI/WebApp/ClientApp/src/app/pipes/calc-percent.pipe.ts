import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'CalcPercent'
})
export class CalcPercentPipe implements PipeTransform {
  transform(value: number, min: number, max: number): number {

    
    if (Number(value) >= Number(max)) {
     
         return 100;

    } else if (Number(value) <= Number(min)) {

           return 0;
  
        } else {
             var result = (100 / (max - min)) * (value - min)
      return Math.ceil(result);
        }

    }
}

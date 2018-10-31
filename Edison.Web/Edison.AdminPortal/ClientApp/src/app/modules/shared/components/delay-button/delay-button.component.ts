import { Component, Output, EventEmitter, Input } from '@angular/core'
import { interval, Subscription, Observable } from 'rxjs'
import { startWith, take } from 'rxjs/operators'

@Component({
  selector: 'app-delay-button',
  templateUrl: './delay-button.component.html',
  styleUrls: ['./delay-button.component.scss'],
})
export class DelayButtonComponent {
  @Input()
  buttonText: string
  @Input()
  subheaderText: string
  @Input()
  disabled = false
  @Input()
  urgency: string
  @Output()
  clickCompleted = new EventEmitter()

  private intervalSub$: Subscription

  currentProgress = 0
  maxProgress = 100
  progressMaxed = false
  progressInterval: any
  hover = false
  active = false
  activated = false

  onMouseDown = () => {
    if (this.disabled) {
      return
    }

    this.active = true

    this.intervalSub$ = interval(500)
      .pipe(
        startWith(1),
        take(9)
      )
      .subscribe(x => {
        if (this.currentProgress >= this.maxProgress) {
          this.intervalSub$.unsubscribe()
          this.progressMaxed = true
          this.clickCompleted.emit()
        } else {
          this.currentProgress += this.maxProgress / 8
        }
      })
  }

  onMouseUp = () => {
    if (this.disabled) {
      return
    }

    this.intervalSub$ && this.intervalSub$.unsubscribe()

    this.active = false
    this.currentProgress = 0
  }

  getCircleClass() {
    return `${this.active ? 'active' : ''} ${this.urgency &&
      this.urgency.toLowerCase()}`
  }
}

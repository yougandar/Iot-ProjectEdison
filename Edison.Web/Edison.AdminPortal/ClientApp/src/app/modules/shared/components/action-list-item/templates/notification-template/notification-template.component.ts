import {
  Component,
  Input,
  ChangeDetectionStrategy,
  EventEmitter,
} from '@angular/core'
import { ActionPlanNotificationAction } from '../../../../../../reducers/action-plan/action-plan.model'

@Component({
  selector: 'app-notification-template',
  templateUrl: './notification-template.component.html',
  styleUrls: ['./notification-template.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotificationTemplateComponent {
  @Input()
  context: ActionPlanNotificationAction

  @Input()
  last: boolean

  @Input()
  canEdit: boolean

  @Input()
  canUpdate: boolean

  @Input()
  onchange: EventEmitter<any>

  notificationText: string
  editing = false
  adding = false

  addNew() {
    this.notificationText = ''
    this.adding = true
  }

  remove() {
    this.notificationText = ''
    this.adding = false
  }

  edit() {
    this.editing = true
  }

  editComplete() {
    this.editing = false
    this.onchange.emit()
  }

  notificationChanged() {
    if (!this.canUpdate) {
      return
    }
    this.onchange.emit()
  }
}

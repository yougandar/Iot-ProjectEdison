import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  OnDestroy,
} from '@angular/core'
import { fadeInOut } from '../../../../core/animations/fadeInOut'
import { Store, select } from '@ngrx/store'
import { AppState } from '../../../../reducers'
import {
  eventsSelector,
  activeEventSelector,
} from '../../../../reducers/event/event.selectors'
import {
  ActionPlan,
  ActionPlanStatus,
} from '../../../../reducers/action-plan/action-plan.model'
import {
  selectedActionPlanSelector,
  actionPlansSelector,
} from '../../../../reducers/action-plan/action-plan.selectors'
import {
  SelectActionPlan,
  GetActionPlans,
  GetActionPlan,
  SetSelectingActionPlan,
} from '../../../../reducers/action-plan/action-plan.actions'
import { PostNewResponse } from '../../../../reducers/response/response.actions'
import { Event } from '../../../../reducers/event/event.model'
import { SelectActiveEvent } from '../../../../reducers/event/event.actions'
import { SearchListItem } from '../../../../core/models/searchListItem'
import { Subscription } from 'rxjs'
import { responsesSelector } from '../../../../reducers/response/response.selectors'
import { Response } from '../../../../reducers/response/response.model'

@Component({
  selector: 'app-activate-response',
  templateUrl: './activate-response.component.html',
  styleUrls: ['./activate-response.component.scss'],
  animations: [fadeInOut],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class ActivateResponseComponent implements OnInit, OnDestroy {
  hover: boolean
  active = false
  disabled = false
  selectedActionPlan: ActionPlan = null
  showActionPlan = false
  activeEvent: Event
  activeResponse: Response
  listItems: SearchListItem[]
  actionPlans: ActionPlan[]
  activated = false

  disabledUntilImplemented = true

  private actionPlansSub$: Subscription
  private eventsSub$: Subscription
  private activeEventSub$: Subscription
  private responsesSub$: Subscription

  constructor(private store: Store<AppState>) {}

  ngOnInit() {
    this.actionPlansSub$ = this.store
      .pipe(select(actionPlansSelector))
      .subscribe(actionPlans => {
        this.listItems = actionPlans.map(ap => ({
          name: ap.name,
          id: ap.actionPlanId,
          icon: ap.icon || '',
          color: ap.color || '',
        }))
        this.actionPlans = actionPlans
      })

    this.eventsSub$ = this.store
      .pipe(select(eventsSelector))
      .subscribe(events => (this.disabled = events.length <= 0))

    this.actionPlansSub$ = this.store
      .pipe(select(selectedActionPlanSelector))
      .subscribe(actionPlan => {
        if (actionPlan) {
          this.selectedActionPlan = actionPlan
        } else {
          this.showActionPlan = false
        }
      })

    this.activeEventSub$ = this.store
      .pipe(select(activeEventSelector))
      .subscribe(event => {
        if (event) {
          this.active = true
          this.activated = false
          this.activeEvent = event
          this.store.dispatch(
            new SetSelectingActionPlan({ isSelecting: this.active })
          )
        }
      })

    this.responsesSub$ = this.store
      .pipe(select(responsesSelector))
      .subscribe(responses => {
        if (this.activeEvent && this.selectedActionPlan) {
          this.activeResponse = responses.find(
            response =>
              // response.actionPlan.actionPlanId === this.selectedActionPlan.actionPlanId &&
              response.primaryEventClusterId === this.activeEvent.eventClusterId
          )
        }
      })

    this.store.dispatch(new GetActionPlans())
  }

  ngOnDestroy() {
    this.actionPlansSub$.unsubscribe()
    this.activeEventSub$.unsubscribe()
    this.eventsSub$.unsubscribe()
    this.responsesSub$.unsubscribe()
  }

  selectActionPlan = (item: SearchListItem) => {
    let actionPlan: ActionPlan = null
    if (item) {
      actionPlan = this.actionPlans.find(ap => ap.actionPlanId === item.id)
      if (!actionPlan.openActions) {
        this.store.dispatch(
          new GetActionPlan({ actionPlanId: actionPlan.actionPlanId })
        )
      }
      this.showActionPlan = true
    }

    this.store.dispatch(new SelectActionPlan({ actionPlan }))
  }

  activateActionPlan = () => {
    this.store.dispatch(
      new PostNewResponse({
        event: this.activeEvent,
        actionPlan: this.selectedActionPlan,
      })
    )
    this.activated = true
    this.selectedActionPlan = {
      ...this.selectedActionPlan,
      openActions: this.selectedActionPlan.openActions.map(a => ({
        ...a,
        status: ActionPlanStatus.Complete,
      })),
    }
  }

  onReturnToMapClick() {
    this.toggleActive()
  }

  toggleActive() {
    if (this.disabled) {
      return
    }

    if (this.active) {
      this.active = false
      this.activated = false
      this.store.dispatch(new SelectActionPlan({ actionPlan: null }))
      this.store.dispatch(new SelectActiveEvent({ event: null }))
    } else if (!this.disabledUntilImplemented) {
      this.active = true
    }

    this.store.dispatch(
      new SetSelectingActionPlan({ isSelecting: this.active })
    )
  }
}

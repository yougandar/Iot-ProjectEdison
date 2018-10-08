import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { fadeInOut } from '../../../../shared/animations/fadeInOut';
import { Store, select } from '@ngrx/store';
import { AppState } from '../../../../reducers';
import { eventsSelector, activeEventSelector } from '../../../../reducers/event/event.selectors';
import { ActionPlan } from '../../../../reducers/action-plan/action-plan.model';
import { selectedActionPlanSelector, actionPlansSelector } from '../../../../reducers/action-plan/action-plan.selectors';
import { SelectActionPlan, GetActionPlans, GetActionPlan } from '../../../../reducers/action-plan/action-plan.actions';
import { PostNewResponse } from '../../../../reducers/response/response.actions';
import { Event } from '../../../../reducers/event/event.model';
import { SelectActiveEvent } from '../../../../reducers/event/event.actions';
import { SearchListItem } from '../../../../shared/models/searchListItem';
import { listFadeInOut } from '../../../../shared/animations/listFadeInOut';

@Component({
  selector: 'app-activate-response',
  templateUrl: './activate-response.component.html',
  styleUrls: ['./activate-response.component.scss'],
  animations: [
    fadeInOut,
  ],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class ActivateResponseComponent implements OnInit {
  hover: boolean;
  active = false;
  disabled = false;
  selectedActionPlan: ActionPlan = null;
  showActionPlan = false;
  activeEvent: Event;
  listItems: SearchListItem[];
  actionPlans: ActionPlan[];

  constructor(private store: Store<AppState>) {}

  ngOnInit() {
    this.store.pipe(select(actionPlansSelector)).subscribe(actionPlans => {
      this.listItems = actionPlans.map(ap => ({
        name: ap.name,
        id: ap.actionPlanId,
        icon: ap.icon || '',
        color: ap.color || '',
      }));
      this.actionPlans = actionPlans;
    });

    this.store.pipe(select(eventsSelector)).subscribe(events => this.disabled = events.length <= 0);
    this.store.pipe(select(selectedActionPlanSelector)).subscribe(actionPlan => {
      if (actionPlan) {
        this.selectedActionPlan = actionPlan;
      } else {
        this.showActionPlan = false;
      }
    });
    this.store.pipe(select(activeEventSelector)).subscribe(event => {
      if (event) {
        this.active = true;
        this.activeEvent = event;
      }
    });

    this.store.dispatch(new GetActionPlans());
  }

  selectActionPlan = (item: SearchListItem) => {
    let actionPlan: ActionPlan = null;
    if (item) {
      actionPlan = this.actionPlans.find(ap => ap.actionPlanId === item.id);
      if (!actionPlan.openActions) {
        this.store.dispatch(new GetActionPlan({ actionPlanId: actionPlan.actionPlanId }));
      }
      this.showActionPlan = true;
    }

    this.store.dispatch(new SelectActionPlan({actionPlan}));
  }

  activateActionPlan = () => {
    this.store.dispatch(new PostNewResponse({ event: this.activeEvent, actionPlan: this.selectedActionPlan }));
    this.toggleActive();
  }

  toggleActive() {
    if (this.disabled) { return; }

    if (this.active) {
      this.active = false;
      this.store.dispatch(new SelectActionPlan({ actionPlan: null }));
      this.store.dispatch(new SelectActiveEvent({ event: null}));
    } else {
      this.active = true;
    }
  }

}

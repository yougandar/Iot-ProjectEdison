import { Component, ViewChild, Input, OnChanges, AfterViewInit } from '@angular/core'
import { Message } from '../../../../reducers/chat/chat.model';
import { NgxAutoScroll } from 'ngx-auto-scroll';

@Component({
    selector: 'app-message-list',
    templateUrl: './message-list.component.html',
    styleUrls: [ './message-list.component.scss' ]
})
export class MessageListComponent implements AfterViewInit, OnChanges {
    @ViewChild(NgxAutoScroll) ngxAutoScroll: NgxAutoScroll;

    @Input()
    messages: Message[];

    constructor () { }

    ngAfterViewInit() {
        this.scrollToBottom();
    }

    ngOnChanges() {
        this.scrollToBottom();
    }

    private scrollToBottom() {
        this.ngxAutoScroll.forceScrollDown();
    }
}

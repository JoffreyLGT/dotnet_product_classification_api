import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import { SidebarComponent } from "./shared/sidebar/sidebar.component";
import { NgxEchartsDirective } from "ngx-echarts";

@Component({
  selector: "app-root",
  imports: [RouterOutlet, SidebarComponent, NgxEchartsDirective],
  template: `
    <div class="flex">
      <app-sidebar />
      <main class="container overflow-auto bg-base-200">
        <router-outlet></router-outlet>
      </main>
    </div>
  `,
  styleUrl: "./app.component.css",
})
export class AppComponent {
  title = "Product classification";
}

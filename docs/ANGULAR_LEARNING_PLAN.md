# План обучения Angular для .NET-разработчика

Учебный маршрут под вас: **сильный .NET-бэкграунд**, почти нет опыта с JavaScript / TypeScript / Angular. Темп: **1–2 часа в день**. Цель — плавно дойти до UI QuestionApp в [`rewrite/client/`](../rewrite/client/) и понимать **суть Angular**, а не копировать сниппеты вслепую.

Связанный документ общего стека: [STACK_TRANSITION_PLAN.md](STACK_TRANSITION_PLAN.md) (этапы 6–8 — UI). Этот файл **детализирует** Angular-часть: фундамент + дневные задачи по UI проекта.

Ориентир по старому UI (живой прототип поведения): Blazor-страницы в [`QuestionAppUI/Pages/`](../QuestionAppUI/Pages/).

---

## Как пользоваться этим планом

1. Идите **строго по сессиям**. Не перескакивайте Part B → Part C.
2. Одна сессия = один день (1–2 ч). Если не уложились — повторите день, не открывайте следующую.
3. Формат каждой сессии одинаковый:
   - **Цель**
   - **Фокус-концепция** (что именно «прочувствовать»)
   - **Аналогия с .NET / Blazor**
   - **Что изучить** (ссылки)
   - **Упражнение**
   - **DoD** (когда день закрыт)
   - **Самопроверка**
   - **Частые ошибки**
4. Part C (UI проекта) начинайте только когда:
   - закрыт Part B;
   - API из STACK-плана отдаёт хотя бы `GET /api/categories` (этап 2+).
5. Не тащите NgRx, UI-kit (Material «на всё»), SSR — это позже и не нужно для первого UI.

---

## Часть A — Мост «.NET → браузер»

Прочитайте один раз целиком (30–45 мин), потом возвращайтесь к таблице как к шпаргалке.

### A.1. Где живёт UI

| Blazor Server (текущий проект) | Angular SPA |
|--------------------------------|-------------|
| UI на сервере, браузер держит SignalR-circuit | UI целиком в браузере после загрузки JS-бандла |
| Один процесс: UI + данные рядом | Два процесса: `ng serve :4200` и `dotnet run :7xxx` |
| `@inject IQuestionData` прямо в страницу | Компонент → сервис → `HttpClient` → Web API |
| Cookie auth «сама едет» с запросами same-site | JWT обычно кладёте в заголовок сами (interceptor) |

**Суть:** Angular — это клиентское приложение. Оно **не знает** про EF, DbContext, PostgreSQL. Только HTTP и JSON.

### A.2. Таблица соответствий

| .NET / Blazor | Angular / web |
|---------------|---------------|
| Решение / `.csproj` | Angular workspace / `package.json` |
| NuGet | npm (`package.json` + `node_modules`) |
| `Program.cs` DI | `app.config.ts` + `providers` |
| `AddScoped` / `AddSingleton` | `providedIn: 'root'` (часто как singleton в рамках вкладки) |
| `@inject T` | `inject(T)` или constructor DI |
| `.razor` компонент | Component = `.ts` + `.html` + `.css` |
| `[Parameter]` | `@Input()` |
| `EventCallback` | `@Output()` + `EventEmitter` |
| `@page "/details/{id}"` | Router: `path: 'questions/:id'` |
| `@if` / `@foreach` | `@if` / `@for` (современный control flow) |
| `HttpClient` (C#) | `HttpClient` (`@angular/common/http`) |
| `DelegatingHandler` / middleware HTTP | HTTP Interceptor |
| `[Authorize]` на странице | Route `canActivate` guard |
| `IConfiguration["ApiUrl"]` | `environment.ts` → `apiUrl` |
| DTO / record | TypeScript `interface` / `type` |
| `async Task` + `await` | `async`/`await` **или** RxJS `Observable` (в Angular HTTP по умолчанию Observable) |

### A.3. Словарь браузера (минимум)

| Термин | Зачем вам |
|--------|-----------|
| **DOM** | Дерево HTML-элементов; Angular меняет его при изменении состояния |
| **DevTools → Elements** | Смотреть HTML/CSS «как получилось» |
| **DevTools → Network** | Видеть HTTP к API (главный инструмент отладки SPA) |
| **DevTools → Console** | Ошибки TypeScript/runtime, `console.log` |
| **npm** | Менеджер пакетов JS (как NuGet + локальный restore в `node_modules`) |
| **Bundler** | Собирает TS/HTML/CSS в файлы для браузера (`ng serve` / `ng build`) |

### A.4. Порты

- Angular Dev Server: обычно `http://localhost:4200`
- API: `https://localhost:7xxx` или HTTP-порт из `launchSettings.json`

Это **разные origin** → нужен CORS на API (или proxy). В STACK-плане рекомендуется CORS в Development.

---

## Часть B — Фундамент (до UI QuestionApp)

Делайте в отдельной песочнице, **не** в `rewrite/client`:

```powershell
# один раз, папка вне или рядом, например:
ng new learn-playground --routing --style=css --ssr=false
cd learn-playground
ng serve
```

Либо короткие упражнения на [StackBlitz](https://stackblitz.com/) / в MDN Playground — но к сессии B7 лучше перейти на локальный CLI.

---

### Сессия B0 — Настройка и ментальная модель (1 ч)

**Цель:** окружение готово, понятно «что будем учить».

**Фокус-концепция:** SPA = приложение в браузере; Angular — фреймворк с DI, компонентами, роутингом.

**Что изучить:**

- Установить [Node.js LTS](https://nodejs.org/), проверить: `node -v`, `npm -v`
- `npm install -g @angular/cli` → `ng version`
- Пролистать введение: [angular.dev / tutorials](https://angular.dev/tutorials)

**Упражнение:** записать своими словами (5–10 предложений): чем Angular SPA отличается от вашего текущего Blazor Server QuestionApp.

**DoD:**

- [ ] `ng version` работает
- [ ] Есть краткая заметка «Blazor vs Angular» своими словами

**Самопроверка:**

- Где выполняется логика отрисовки списка вопросов в Blazor? А где будет в Angular?

**Частые ошибки:** слишком старый Node; глобальный CLI не совпадает с будущей версией проекта — для проекта потом используйте локальный `npx ng`.

---

### Сессия B1 — HTML: каркас страницы (1–2 ч)

**Цель:** читать и писать простую разметку без страха.

**Фокус-концепция:** HTML описывает **структуру**, не логику.

**Аналогия с .NET:** как `.cshtml` / разметка в `.razor`, но без C# внутри.

**Что изучить:**

- [MDN: HTML basics](https://developer.mozilla.org/en-US/docs/Learn/HTML/Introduction_to_HTML/Getting_started)
- Теги: `html`, `head`, `body`, `h1`, `p`, `a`, `ul`/`li`, `div`, `span`
- Формы: `form`, `input`, `button`, `label`, атрибут `type`

**Упражнение:** один файл `questions.html` (можно открыть в браузере):

- заголовок «QuestionApp»
- список из 3 вопросов (`ul`/`li`)
- ссылка «Подробнее»
- форма: поле «Новый вопрос» + кнопка «Отправить» (без реальной отправки)

**DoD:**

- [ ] Страница открывается в браузере и выглядит структурно понятно
- [ ] Форма есть, кнопки подписаны через `label`

**Самопроверка:**

- Чем `div` отличается от `span`?
- Зачем `label for="..."`?

**Частые ошибки:** забывать закрывать теги; путать атрибут `class` с CSS-файлом.

---

### Сессия B2 — CSS: внешний вид минимум (1–2 ч)

**Цель:** управлять отступами, шрифтом, простой раскладкой.

**Фокус-концепция:** селекторы + cascade; CSS не «магия Angular».

**Аналогия с .NET:** как `site.css` / scoped CSS в Blazor, только синтаксис везде один.

**Что изучить:**

- [MDN: CSS first steps](https://developer.mozilla.org/en-US/docs/Learn/CSS/First_steps)
- Селекторы: элемент, `.class`, `#id`
- `margin`, `padding`, `color`, `font-size`
- Flexbox минимум: `display: flex`, `gap`, `justify-content`
- CSS-переменные: `:root { --accent: #246; }`

**Упражнение:** стилизуйте `questions.html`:

- шапка и список разделены отступами
- кнопка визуально заметна
- на узком окне текст не «ломается» катастрофически

**DoD:**

- [ ] Стили в отдельном `.css`
- [ ] Используете хотя бы одну CSS-переменную и flex для шапки/кнопок

**Самопроверка:**

- Что специфичнее: `p` или `.title`?
- Зачем переменные, если можно хардкодить цвет?

**Частые ошибки:** писать всё в inline `style=""`; учить Grid «на месяц» вместо простых flex-правил.

---

### Сессия B3 — JavaScript: язык без фреймворка (1–2 ч)

**Цель:** базовый синтаксис JS, на котором стоит TypeScript.

**Фокус-концепция:** JS — динамически типизированный язык; объекты и функции — граждане первого класса.

**Аналогия с .NET:**

| C# | JavaScript |
|----|------------|
| `var x` / тип обязателен чаще | `let` / `const` (тип не обязателен) |
| `void Foo()` | `function foo() {}` или `const foo = () => {}` |
| `List<T>` | `Array` (`[]`) |
| `new MyClass { Prop = 1 }` | object literal `{ prop: 1 }` |
| `null` | `null` и отдельно `undefined` |

**Что изучить:**

- [MDN: JS first steps](https://developer.mozilla.org/en-US/docs/Learn/JavaScript/First_steps)
- `const` / `let` (не `var`)
- функции, стрелочные функции
- массивы: `map`, `filter`, `find`
- объекты, доступ через `.` и `["key"]`

**Упражнение:** в Console браузера или в файле `.js`:

```js
const questions = [
  { id: 1, title: 'What is DI?', approved: true },
  { id: 2, title: 'Explain LINQ', approved: false },
];
// получить только approved и вывести title через map/filter
```

**DoD:**

- [ ] Можете без подсказки написать `filter` + `map` для массива объектов
- [ ] Понимаете разницу `const` объекта и мутации его полей

**Самопроверка:**

- Почему `const user = {}; user.name = 'A'` законно?
- Чем `==` опаснее `===`?

**Частые ошибки:** учить jQuery-историю 2010-х; игнорировать `===`.

---

### Сессия B4 — JavaScript async: Promise и fetch (1–2 ч)

**Цель:** понять асинхронность в браузере — это фундамент `HttpClient`.

**Фокус-концепция:** сеть не мгновенна; результат придёт **потом** (Promise / async-await).

**Аналогия с .NET:** `Task` / `async await`. `fetch` ≈ «тонкий HttpClient».

**Что изучить:**

- [MDN: Promises](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise)
- [MDN: Using fetch](https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch)
- `async` / `await`, `try/catch`

**Упражнение:** запросите публичный JSON (например `https://jsonplaceholder.typicode.com/posts?_limit=3`) через `fetch` + `await`, выведите заголовки в `console.log`. Затем оберните в `try/catch` и сломайте URL — поймайте ошибку.

**DoD:**

- [ ] Есть рабочий `async`-пример с `fetch`
- [ ] Понимаете: после `await` код ниже выполнится только когда ответ готов

**Самопроверка:**

- Чем Promise похож на `Task`?
- Что будет, если забыть `await`?

**Частые ошибки:** думать, что Angular «вместо» async — нет, он **поверх**; путать синхронный код с сетевым.

---

### Сессия B5 — TypeScript: типы как в C#, но не совсем (1–2 ч)

**Цель:** писать интерфейсы и типобезопасные функции.

**Фокус-концепция:** TypeScript = JavaScript + типы на этапе компиляции (стираются в JS).

**Аналогия с .NET:**

| C# | TypeScript |
|----|------------|
| `interface IUser` / DTO | `interface User` |
| `string?` | `string \| null` или `string?` (strictNullChecks) |
| `List<QuestionDto>` | `QuestionDto[]` |
| `enum` | `enum` или union `'User' \| 'Admin'` |
| generics `Repository<T>` | `function identity<T>(x: T): T` |

**Что изучить:**

- [TypeScript Handbook: Everyday Types](https://www.typescriptlang.org/docs/handbook/2/everyday-types.html)
- `interface`, optional `?`, union types
- type assertions осторожно (`as`)

**Упражнение:** опишите DTO вопроса:

```ts
interface QuestionListItem {
  id: number;
  title: string;
  authorName: string;
  categoryNames: string[];
  dateCreated: string; // ISO из API
}
```

Функция `function approvedOnly(items: QuestionListItem[], approvedFlag: boolean): QuestionListItem[]`.

Проверьте в [TypeScript Playground](https://www.typescriptlang.org/play).

**DoD:**

- [ ] Интерфейс без `any`
- [ ] Компилятор ругается, если вернуть не тот тип — и вы понимаете сообщение

**Самопроверка:**

- Чем `interface` в TS отличается от C# interface с точки зрения runtime?
- Почему `any` — плохая привычка для обучения?

**Частые ошибки:** везде `any`; путать compile-time типы с проверками на сервере.

---

### Сессия B6 — TypeScript: modules и import/export (1 ч)

**Цель:** понимать, как файлы ссылаются друг на друга (критично для Angular).

**Фокус-концепция:** каждый файл — модуль; явные `import`/`export` вместо «всего в namespace».

**Аналогия с .NET:** `using` + сборки, но зависимость **файловая**; нет единого огромного namespace по умолчанию.

**Что изучить:**

- [TS Modules](https://www.typescriptlang.org/docs/handbook/modules.html)
- `export interface`, `export class`, `import { X } from './x'`
- default export vs named export (в Angular чаще named)

**Упражнение:** два файла: `models.ts` + `app.ts`; импорт интерфейса; функция печатает title.

**DoD:**

- [ ] Можете объяснить ошибку `Cannot find module` / `is not exported`

**Самопроверка:**

- Зачем в Angular почти везде `import { Component } from '@angular/core'`?

**Частые ошибки:** циклические импорты; default export «по привычке», когда команда ждёт named.

---

### Сессия B7 — Angular CLI и скелет приложения (1–2 ч)

**Цель:** создать приложение и понять входную точку.

**Фокус-концепция:** CLI генерирует структуру; `bootstrapApplication` запускает корневой компонент.

**Аналогия с .NET:** `dotnet new web` + `Program.cs` → у Angular `main.ts` + `app.config.ts`.

**Что изучить:**

- [angular.dev: Local setup](https://angular.dev/tools/cli/setup-local)
- [Application structure](https://angular.dev/guide/essentials)

**Упражнение:**

```powershell
ng new learn-playground --routing --style=css --ssr=false
cd learn-playground
ng serve
```

Откройте и прочитайте (не обязательно править):

- `package.json` — зависимости (как csproj PackageReference)
- `src/main.ts` — точка входа
- `src/app/app.config.ts` — провайдеры (как DI registration)
- `src/app/app.ts` / `app.component.*` — корневой компонент
- `angular.json` — проектные настройки сборки

**DoD:**

- [ ] Страница `localhost:4200` открывается
- [ ] Можете устно сказать, какой файл «как Program.cs»

**Самопроверка:**

- Что такое standalone component?
- Зачем `node_modules` не коммитят?

**Частые ошибки:** путать глобальный `ng` и версию в проекте; коммитить `node_modules`.

---

### Сессия B8 — Component: кирпич UI (1–2 ч)

**Цель:** создать свой компонент и вывести данные.

**Фокус-концепция:** компонент = класс + шаблон + (опционально) стили; UI строится деревом компонентов.

**Аналогия с .NET:** Blazor component = `.razor` + code-behind; в Angular чаще три файла или один с `template:`.

**Что изучить:**

- [Components](https://angular.dev/guide/components)
- `@Component({ selector, templateUrl, styleUrl })`
- Интерполяция `{{ title }}`

**Упражнение:**

```powershell
ng generate component features/question-card --standalone
```

В компоненте поле `title = 'What is DI?'`, в шаблоне показать в `<h2>{{ title }}</h2>`. Вставить `<app-question-card />` в корневой шаблон.

**DoD:**

- [ ] Видите свой компонент на странице
- [ ] Понимаете, что меняя поле класса, меняется текст в шаблоне (после механизма обновления UI)

**Самопроверка:**

- Что такое `selector`?
- Где «logic», где «markup»?

**Частые ошибки:** забыть объявить/импортировать компонент в родителе (в standalone — `imports: [QuestionCardComponent]`).

---

### Сессия B9 — Binding: связь шаблона и класса (1–2 ч)

**Цель:** освоить три вида привязки — это ежедневный инструмент Angular.

**Фокус-концепция:**

| Синтаксис | Направление | Пример |
|-----------|-------------|--------|
| `{{ value }}` | class → DOM текст | как текст в Razor |
| `[attr]="value"` / `[disabled]="isBusy"` | class → property | property binding |
| `(click)="save()"` | DOM → class | event binding |
| `[(ngModel)]="name"` | двусторонне | похоже на `@bind` |

**Аналогия с Blazor:** `@bind`, `@onclick`, атрибуты на элементах.

**Что изучить:**

- [Template syntax](https://angular.dev/guide/templates)
- [Event binding](https://angular.dev/guide/templates/event-binding)

**Упражнение:** счётчик:

- кнопка `(click)="inc()"` увеличивает `count`
- `[disabled]="count >= 10"`
- текст `{{ count }}`

Затем поле ввода с `[(ngModel)]` (потребуется `FormsModule` в `imports` компонента).

**DoD:**

- [ ] Счётчик работает
- [ ] Можете объяснить разницу `[value]` и `(input)` vs `[(ngModel)]`

**Самопроверка:**

- Почему `(click)="save()"` с скобками, а не `save`?
- Что будет, если написать `click="save()"` без `()` Angular-синтаксиса?

**Частые ошибки:** путать `[]` и `()`; забывать импорт `FormsModule` для `ngModel`.

---

### Сессия B10 — Control flow: @if / @for / @switch (1–2 ч)

**Цель:** условный рендер и списки — основа экранов QuestionApp.

**Фокус-концепция:** шаблон декларативно описывает UI от состояния; список = `@for` по массиву.

**Аналогия с .NET:** Razor `@if` / `@foreach`. Старый синтаксис Angular `*ngIf` / `*ngFor` ещё встречается в статьях — учите **новый** `@if` / `@for`.

**Что изучить:**

- [Control flow](https://angular.dev/guide/templates/control-flow)
- track в `@for` (зачем `track item.id`)

**Упражнение:** массив `questions` в компоненте; `@for` рисует карточки; `@if (questions.length === 0)` — «пусто». Кнопка добавляет элемент в массив.

**DoD:**

- [ ] Список обновляется при добавлении
- [ ] Используете `track` по `id`

**Самопроверка:**

- Зачем `track`? (намёк: переиспользование DOM-узлов)
- Где хранится «источник правды» — в шаблоне или в классе?

**Частые ошибки:** мутировать массив неправильно так, что UI не видит изменений (для обучения пока достаточно создавать новый массив `[...items, newItem]`).

---

### Сессия B11 — Services и DI (1–2 ч)

**Цель:** вынести логику из компонента — как Application Service в .NET.

**Фокус-концепция:** сервис — `@Injectable`; компонент **тонкий**, сервис переиспользуемый; DI встроен в Angular.

**Аналогия с .NET:**

```csharp
// Program.cs
builder.Services.AddScoped<IQuestionService, QuestionService>();
// Page
@inject IQuestionService Questions
```

```ts
@Injectable({ providedIn: 'root' })
export class QuestionService { ... }

private readonly questions = inject(QuestionService);
```

**Что изучить:**

- [Dependency injection](https://angular.dev/guide/di)
- `inject()` function vs constructor injection
- `providedIn: 'root'`

**Упражнение:** `QuestionService` с методом `getMockQuestions(): Question[]`. Компонент списка только вызывает сервис и рисует. Второй компонент тоже использует тот же сервис — убедитесь, что это один экземпляр (`providedIn: 'root'`).

**DoD:**

- [ ] Два компонента используют один сервис
- [ ] Можете объяснить, зачем не класть HTTP/бизнес-логику в шаблон

**Самопроверка:**

- Чем `providedIn: 'root'` похож на Singleton в рамках вкладки?
- Когда сервис лучше предоставить на уровне компонента (редко на старте)?

**Частые ошибки:** создавать сервис через `new QuestionService()` вручную — ломаете DI и тестируемость.

---

### Сессия B12 — Router: много «страниц» в SPA (1–2 ч)

**Цель:** навигация без полной перезагрузки.

**Фокус-концепция:** URL → компонент; `router-outlet` — точка вставки; params из URL.

**Аналогия с .NET:** `@page "/details/{id}"` + `NavigationManager`.

**Что изучить:**

- [Routing](https://angular.dev/guide/routing)
- `Routes`, `RouterLink`, `ActivatedRoute`
- `withComponentInputBinding()` (удобно для `:id` как `@Input`) — можно на следующем круге

**Упражнение:**

- маршруты `/`, `/about`, `/items/:id`
- список ссылок `routerLink`
- на details читаете `id` из `ActivatedRoute.snapshot.paramMap` или `paramMap` subscribe

**DoD:**

- [ ] URL меняется, компонент тоже, страница не перезагружается целиком как multi-page сайт
- [ ] Param `id` читается и показывается

**Самопроверка:**

- Что рендерится внутри `router-outlet`?
- Чем SPA-route отличается от физического `.html` файла на диске?

**Частые ошибки:** обычный `href="/about"` вместо `routerLink` (полная перезагрузка); забыть объявить маршруты в `app.routes.ts`.

---

### Чеклист закрытия Part B

Прежде чем Part C:

- [ ] B0–B12 DoD отмечены
- [ ] Можете без шпаргалки объяснить: component, binding, service/DI, router, async/fetch
- [ ] Песочница `learn-playground` запускается
- [ ] API категорий для проекта готово (STACK этап 2+) — иначе C6.3 будет стоять

---

## Часть C — UI QuestionApp (этапы 6–8 STACK)

Работайте в [`rewrite/client/`](../rewrite/client/). Каждая сессия ≈ **1 день (1–2 ч)**.  
Общий контекст этапов: [STACK_TRANSITION_PLAN.md](STACK_TRANSITION_PLAN.md) § этапы 6–8.

Предусловие: Web API запущен, Swagger показывает нужные endpoints.

---

# Блок C6 — Этап 6: Angular Hello + API

Соответствует STACK «Этап 6». Цель блока: категории с PostgreSQL видны в Angular.

---

### Сессия C6.1 — Создать `rewrite/client` и разобрать скелет (1–2 ч)

**Цель:** проект клиента QuestionApp существует.

**Фокус-концепция:** структура реального приложения vs песочница.

**Аналогия:** `dotnet new` в `rewrite/src` → `ng new` в `rewrite/client`.

**Упражнение:**

```powershell
cd rewrite
ng new client --routing --style=css --ssr=false
cd client
ng serve
```

Пройдитесь по `main.ts`, `app.config.ts`, `app.routes.ts`. Запишите в заметки: «где регистрировать HttpClient».

**Ориентир Blazor:** сам факт отдельного UI-host — [`QuestionAppUI`](../QuestionAppUI/).

**DoD:**

- [ ] `http://localhost:4200` открывается из `rewrite/client`
- [ ] Понимаете роль `app.config.ts`

**Частые ошибки:** создать Angular в корне репозитория вместо `rewrite/client`.

---

### Сессия C6.2 — environment и apiUrl (1 ч)

**Цель:** базовый URL API не захардкожен в каждом сервисе.

**Фокус-концепция:** конфигурация окружения (Dev vs Prod).

**Аналогия:** `appsettings.Development.json` → `environment.ts` / `environment.development.ts`.

**Что изучить:** [file replacements / application environments](https://angular.dev/tools/cli/environments) (достаточно понять идею `apiUrl`).

**Упражнение:** добавьте `src/environments/environment.ts`:

```ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:PORT', // HTTP-порт API из launchSettings
};
```

Сервис пока только читает `environment.apiUrl` в `console.log` при старте.

**DoD:**

- [ ] Один файл конфигурации с `apiUrl`
- [ ] Порт совпадает с реально запущенным API

**Частые ошибки:** указать HTTPS с self-signed и утонуть в ошибках сертификата на первом дне — для учёбы берите HTTP-порт API.

---

### Сессия C6.3 — provideHttpClient и GET категорий (1–2 ч)

**Цель:** первый реальный HTTP из Angular в ваш .NET API.

**Фокус-концепция:** `HttpClient.get<T>()` возвращает **Observable**; подписка или `async`/`firstValueFrom` запускает запрос.

**Аналогия:** C# `HttpClient.GetFromJsonAsync<T>()`.  
В Angular часто:

```ts
this.http.get<Category[]>(`${environment.apiUrl}/api/categories`)
  .subscribe(data => this.categories = data);
```

**Что изучить:**

- [HttpClient](https://angular.dev/guide/http)
- Зачем `provideHttpClient()` в `app.config.ts`

**Упражнение:**

1. `provideHttpClient()` в конфиге.
2. `CategoryService` с `getAll()`.
3. Компонент вызывает сервис в `ngOnInit` (или конструкторе + явный метод `load()` — лучше явный `load()` из `ngOnInit`).
4. Пока выведите JSON через `{{ categories | json }}` (нужен `JsonPipe`).

**DoD:**

- [ ] В Network tab есть `GET .../api/categories` → 200
- [ ] Данные с API (не захардкоженный массив)

**Самопроверка:**

- Что будет, если вызвать `get()` и **не** подписаться / не превратить в Promise?
- Чем Observable похож и не похож на `Task`?

**Частые ошибки:** забыли `provideHttpClient`; подписались дважды и сделали два запроса; словили CORS (см. C6.4).

---

### Сессия C6.4 — CORS и отладка Network (1–2 ч)

**Цель:** научиться читать ошибки кросс-домена и отличать их от 500 API.

**Фокус-концепция:** браузер блокирует ответ, если API не разрешил origin `http://localhost:4200`.

**Аналогия:** это не «баг Angular»; это политика браузера. Swagger на том же host, что API, CORS не показывает проблему.

**Упражнение:**

1. На API включите CORS для `http://localhost:4200` (как в STACK этап 6).
2. Специально сломайте origin — посмотрите текст ошибки в Console.
3. Исправьте обратно.
4. Научитесь в Network смотреть: Status, Request Headers, Response.

**DoD:**

- [ ] Можете отличить CORS-ошибку от 404/500
- [ ] Список категорий снова грузится

**Самопроверка:**

- Почему Postman «работает», а браузер «нет» при выключенном CORS?

**Частые ошибки:** чинить CORS на Angular вместо API; путать preflight `OPTIONS` с основным `GET`.

---

### Сессия C6.5 — Список категорий: loading и error (1–2 ч)

**Цель:** UI-состояния, без которых SPA выглядит «сломанной».

**Фокус-концепция:** явное состояние экрана: `loading | error | success`.

**Аналогия с Blazor:** флаги `isLoading`, сообщения об ошибке на странице.

**Упражнение:** в компоненте:

```ts
categories: Category[] = [];
loading = false;
error: string | null = null;
```

В шаблоне:

- `@if (loading) { ... }`
- `@if (error) { ... }`
- `@for (c of categories; track c.id) { ... }`

На время теста остановите API — должна появиться ошибка, не «тишина».

**DoD:**

- [ ] Видны loading и обработка ошибки
- [ ] Успешный список — имя + description категории

**Частые ошибки:** оставлять вечный loading, если ошибка не сбрасывает флаг в `finalize`/`catch`.

---

### Сессия C6.6 — TypeScript-модель Category vs C# DTO (1 ч)

**Цель:** контракт фронта совпадает с API.

**Фокус-концепция:** TS `interface` описывает **форму JSON**, не EF-сущность.

**Аналогия:** C# DTO на API ↔ `interface Category` на клиенте. Имена свойств обычно **camelCase** в JSON.

**Упражнение:** сверьте ответ Swagger/Network с интерфейсом. Если API отдаёт `name`, в TS должно быть `name: string`. Поправьте расхождения. Уберите `any`.

**Ориентир модели:** [`CategoryModel.cs`](../QuestionAppLibrary/Models/CategoryModel.cs) (поля), но PK уже `number` в новом API.

**DoD:**

- [ ] Интерфейс без `any`, поля совпадают с JSON
- [ ] Можете объяснить, почему не импортируете C# класс в Angular

**Чеклист закрытия C6 (STACK этап 6):**

- [ ] Категории с PostgreSQL на экране
- [ ] CORS понятен
- [ ] Сервис + компонент разделены

---

# Блок C7 — Этап 7: экраны-минимум

Соответствует STACK «Этап 7». Ориентиры Blazor: [`Index.razor`](../QuestionAppUI/Pages/Index.razor), [`Details.razor`](../QuestionAppUI/Pages/Details.razor).

---

### Сессия C7.1 — Routing layout приложения (1–2 ч)

**Цель:** каркас маршрутов продукта.

**Фокус-концепция:** layout + `router-outlet`; навигация между экранами.

**Упражнение:** маршруты:

```text
/                → QuestionListComponent
/questions/:id   → QuestionDetailsComponent (заглушка)
/login           → LoginComponent (заглушка)
```

Общая шапка: название QuestionApp + ссылки `routerLink`.

**DoD:**

- [ ] Переключение маршрутов без перезагрузки
- [ ] Заглушки открываются

**Частые ошибки:** дублировать шапку в каждом компоненте вместо общего layout.

---

### Сессия C7.2 — Список одобренных вопросов + сервис (1–2 ч)

**Цель:** главный экран гостя.

**Фокус-концепция:** сервис на feature (`QuestionService`) + list DTO.

**Ориентир:** [`Index.razor`](../QuestionAppUI/Pages/Index.razor).

**Упражнение:**

- `GET /api/questions?approved=true` (или ваш контракт из STACK этапа 4)
- интерфейс `QuestionListItem`
- список: title, author, categories, дата
- клик / `routerLink` на детали

**DoD:**

- [ ] Реальные вопросы с API
- [ ] Пустой список обрабатывается (`@if`)

**Частые ошибки:** тащить в список тяжёлый details-DTO «на всякий случай».

---

### Сессия C7.3 — Details и params маршрута (1–2 ч)

**Цель:** экран одного вопроса.

**Фокус-концепция:** параметр URL — входные данные экрана; при смене `id` нужна перезагрузка данных.

**Ориентир:** [`Details.razor`](../QuestionAppUI/Pages/Details.razor).

**Что изучить:** `ActivatedRoute`, `paramMap`; подписка vs `snapshot` (snapshot опасен при навигации id1→id2 на том же компоненте).

**Упражнение:** `GET /api/questions/:id`; показать title, description, author, categories. 404 → сообщение.

**DoD:**

- [ ] Детали грузятся по id из URL
- [ ] Несуществующий id не роняет приложение

**Самопроверка:** когда `snapshot` недостаточен?

---

### Сессия C7.4 — Login: template-driven форма (1–2 ч)

**Цель:** первая форма пользователя (проще reactive для старта).

**Фокус-концепция:** `ngModel` + `FormsModule`; форма собирает данные для `POST /api/auth/login`.

**Аналогия:** модель логина + POST как в [`Login.cshtml.cs`](../QuestionAppUI/Areas/Identity/Pages/Account/Login.cshtml.cs), но ответ — JWT, не cookie.

**Упражнение:** поля userName/password; кнопка вызывает `AuthService.login()`. Пока достаточно `console.log` токена / показать «успех|ошибка».

**DoD:**

- [ ] Успешный логин демо-пользователя против реального API
- [ ] Неверный пароль показывает ошибку

**Частые ошибки:** начать сразу с сложных reactive forms и застрять; не обработать 401.

---

### Сессия C7.5 — JWT в localStorage и AuthService (1–2 ч)

**Цель:** сессия клиента после логина.

**Фокус-концепция:** токен — строка; хранение в `localStorage` (учебный вариант); сервис инкапсулирует get/set/clear + `isLoggedIn`.

**Аналогия:** не Static-класс с глобальным состоянием «как получится», а явный сервис (как scoped/singleton auth state). Cookie Blazor «ехала сама»; JWT **не** приедет сам.

**Упражнение:**

- после login сохранить token
- метод `logout()` чистит storage
- шапка показывает UserName / кнопку Logout
- обновить страницу — «логин» восстанавливается из storage

**DoD:**

- [ ] F5 сохраняет видимость «залогинен»
- [ ] Logout очищает токен

**Самопроверка:** почему токен в JS-памяти без storage пропадёт после F5?

**Частые ошибки:** хранить пароль; класть токен в несколько разных ключей хаотично.

---

### Сессия C7.6 — HTTP Interceptor (1–2 ч)

**Цель:** автоматически добавлять `Authorization: Bearer ...`.

**Фокус-концепция:** interceptor = pipeline для всех HTTP-вызовов (как `DelegatingHandler` в .NET).

**Что изучить:** [HTTP interceptors](https://angular.dev/guide/http/interceptors) (functional interceptors в современном Angular).

**Упражнение:**

- interceptor читает токен из `AuthService` и клонирует request с заголовком
- зарегистрировать через `provideHttpClient(withInterceptors([...]))`
- вызвать защищённый endpoint — в Network виден заголовок

**DoD:**

- [ ] Защищённый API-метод проходит с токеном без ручной установки headers в каждом сервисе
- [ ] Без токена — 401 (ожидаемо)

**Частые ошибки:** забыть `clone`; добавить interceptor, но не через `withInterceptors`.

---

### Сессия C7.7 — Route Guard (1–2 ч)

**Цель:** не пускать гостя на `/create` (или аналог).

**Фокус-концепция:** guard решает, активировать ли маршрут — **UX-защита**; настоящая безопасность только на API.

**Аналогия:** `[Authorize]` на Blazor-странице + всё равно проверка на сервере.

**Упражнение:** `authGuard`: если нет токена → `router.parseUrl('/login')`. Навесить на create-route.

**DoD:**

- [ ] Гость редиректится на login
- [ ] С токеном страница открывается
- [ ] Прямой вызов API без токена всё ещё 401 (проверяете в Swagger/Network)

**Самопроверка:** можно ли обойти guard? (Да, руками.) Что тогда защищает данные?

---

### Сессия C7.8 — Create question: введение в reactive forms (1–2 ч)

**Цель:** форма создания вопроса на reactive forms (важный навык вакансий).

**Фокус-концепция:** `FormGroup` / `FormControl` / `Validators` — модель формы в TypeScript, шаблон к ней привязан.

**Аналогия:** объект модели + DataAnnotations; reactive forms явнее template-driven для сложных форм.

**Что изучить:** [Reactive forms](https://angular.dev/guide/forms/reactive-forms)

**Упражнение:**

- поля: title, description, categoryIds (мультивыбор упростить: один select на первом круге)
- `POST /api/questions` с Bearer
- успех → navigate на details или list

**Ориентир:** [`Create.razor`](../QuestionAppUI/Pages/Create.razor), [`CreateQuestionModel.cs`](../QuestionAppUI/Models/CreateQuestionModel.cs).

**DoD:**

- [ ] Вопрос создаётся в БД
- [ ] Форма использует `FormGroup`, не только `ngModel`

**Частые ошибки:** забыли `ReactiveFormsModule` / `FormBuilder` provider; не вызвать `markAllAsTouched` при ошибке валидации.

---

### Сессия C7.9 — UX форм: disabled, submit, validation (1–2 ч)

**Цель:** довести create/login до «не стыдно показать».

**Фокус-концепция:** состояние submit (`saving`), блокировка кнопки, сообщения валидации.

**Упражнение:**

- кнопка disabled пока `form.invalid || saving`
- текст ошибок под полями (`required`, `minLength`)
- на login/create — единый стиль сообщений
- после create новый вопрос **не** обязан сразу быть на главной (модерация) — покажите toast/текст «ожидает одобрения»

**DoD:**

- [ ] Нельзя заспамить POST кнопкой
- [ ] Пользователь видит, почему submit не проходит
- [ ] Поведение модерации объяснено в UI одной фразой

**Чеклист закрытия C7 (STACK этап 7):**

- [ ] List + Details + Login + Create
- [ ] JWT + interceptor + guard
- [ ] Формы с валидацией

---

# Блок C8 — Этап 8: ответы, голоса, модерация

Соответствует STACK «Этап 8». Делайте **после** готовых API-срезов 8A/8B/8C. Каждый день = тонкий UI-срез.

---

### Сессия C8A.1 — Модель Answer и загрузка списка (1–2 ч)

**Цель:** на Details видны ответы.

**Фокус-концепция:** вложенные данные экрана (вопрос + коллекция answers) — два запроса или один aggregate DTO; для обучения два запроса нормальны.

**Ориентир:** блок ответов в [`Details.razor`](../QuestionAppUI/Pages/Details.razor).

**Упражнение:** `AnswerService` / методы в `QuestionService`; `@for` по ответам под вопросом.

**DoD:**

- [ ] Ответы с API отображаются
- [ ] Пустой список ответов — понятное сообщение

---

### Сессия C8A.2 — Форма добавления ответа (1–2 ч)

**Цель:** авторизованный пользователь пишет ответ.

**Фокус-концепция:** после POST обновить список (повторный GET или immutable push локально — выберите один способ и придерживайтесь).

**Упражнение:** textarea + submit; только если logged in; иначе ссылка на login.

**DoD:**

- [ ] Ответ появляется без обязательного полного F5 (локальный refresh списка)
- [ ] 401 обработан

---

### Сессия C8B.1 — Кнопка голоса и обновление UI (1–2 ч)

**Цель:** toggle vote.

**Фокус-концепция:** оптимистичный UI vs ждать ответ сервера; для обучения — **дождаться ответа**, затем обновить счётчик из response DTO.

**Упражнение:** кнопка «Upvote»; `POST .../vote`; показать число голосов; повторный клик по контракту API.

**DoD:**

- [ ] Счётчик совпадает с БД после refresh
- [ ] Гость не голосует (UI + 401)

---

### Сессия C8B.2 — Состояние «я уже голосовал» (1 ч)

**Цель:** UI отражает личный статус голоса.

**Фокус-концепция:** derived state из DTO (`votedByMe: boolean`) лучше, чем угадывать на клиенте.

**Упражнение:** стиль кнопки зависит от `votedByMe`; не ломать список при ошибке сети (откат состояния).

**DoD:**

- [ ] После F5 состояние голоса корректно
- [ ] Ошибка сети не оставляет «вечный pressed» без факта в БД

---

### Сессия C8C.1 — Admin-страница pending (1–2 ч)

**Цель:** список вопросов на модерации.

**Фокус-концепция:** feature-area admin; скрыть ссылку не-админам, но **не** считать это защитой.

**Ориентир:** [`AdminApproval.razor`](../QuestionAppUI/Pages/AdminApproval.razor).

**Упражнение:** route `/admin/approval` + guard «залогинен»; данные `GET admin/.../pending`.

**DoD:**

- [ ] Admin видит pending
- [ ] User получает 403 от API (проверить Network)

---

### Сессия C8C.2 — Approve / Reject + роль из JWT (1–2 ч)

**Цель:** действия модерации и чтение роли на клиенте.

**Фокус-концепция:** JWT payload содержит claims (role); парсинг на клиенте **только для UI**; сервер всё равно главный.

**Упражнение:**

- кнопки Approve/Reject
- после успеха убрать элемент из списка
- в шапке показывать Admin-ссылку только если role === `Admin` (парсинг payload или claim из login-response)

**DoD:**

- [ ] Approve делает вопрос видимым на главной
- [ ] UI-ссылка Admin скрыта для User
- [ ] Прямой запрос User к admin API → 403

**Чеклист закрытия C8:**

- [ ] Answers + Votes + Admin UI работают end-to-end с API
- [ ] Понимаете границу «UI convenience» vs «server security»

---

## Часть D — Суть Angular: что изучить глубже

Не отдельный «месяц теории», а **контрольный список**. Возвращайтесь к пункту, когда встретите симптом в проекте.

### D.1. Change detection (интуитивно)

Angular обновляет DOM, когда меняется состояние, на которое подписан шаблон.  
Для старта: меняйте поля компонента / сигналы / результаты subscribe — и UI обновится.  
Если UI «не обновляется» — часто проблема в мутациях, забытом subscribe, или обновлении вне зоны Angular (редко на старте).

Читать позже: [Change detection](https://angular.dev/guide/change-detection) обзорно, без ухода в микрооптимизации.

### D.2. Standalone vs NgModules

Ваш путь — **только standalone**. Старые курсы про `NgModule` / `declarations` могут путать.  
Если статья требует Module — ищите современный аналог на angular.dev.

### D.3. Observable vs Promise / async-await

| | `Task` / Promise | Observable (RxJS) |
|--|------------------|-------------------|
| Результат | один | ноль/один/много |
| Старт | сразу при создании Task/вызове | часто **ленивый** до `subscribe` |
| Отмена | CancellationToken | `unsubscribe` / `takeUntilDestroyed` |

`HttpClient` → Observable. Варианты на обучении:

1. `subscribe` + аккуратная отписка (`takeUntilDestroyed()`)
2. `firstValueFrom(obs)` → `await` в `async` методе
3. `async` pipe в шаблоне (`data$ | async`) — мощно, освоить после subscribe

**Хорошо понять:** запрос не уйдёт без подписки / firstValueFrom.

### D.4. Тонкий компонент / толстый сервис

Компонент: UI-состояние, binding, навигация.  
Сервис: HTTP, map DTO, хранение токена, переиспользуемые правила.  
Это тот же вкус, что «тонкий Controller / толстый Service» в .NET.

### D.5. Signals (ознакомление)

В современном Angular появляются `signal` / `computed`.  
Не блокируйте Part C ради полного курса signals; достаточно знать, что это официальный путь состояния. Можно внедрить точечно после C7.

### D.6. Безопасность

Повторяйте как мантру:

1. Guard и скрытие кнопок — UX.
2. JWT подпись и `[Authorize]` на API — безопасность.
3. XSS: не вставляйте HTML с API через «сырой» binding без санитизации (у вас в Blazor был HtmlSanitizer — та же угроза на SPA).

---

## Часть E — Календарь (ориентир)

При **1–2 часа в день**:

| Блок | Сессии | Ориентир по времени |
|------|--------|---------------------|
| Part A (чтение) | — | 1 день |
| Part B фундамент | B0–B12 | 2–3 недели |
| C6 Hello API | C6.1–C6.6 | ~1 неделя |
| C7 экраны | C7.1–C7.9 | ~1.5–2 недели |
| C8 срезы | C8A–C8C | ~1–2 недели |
| **Итого до рабочего UI ядра** | | **≈ 6–9 недель** |

Параллельно с Part B можно делать backend STACK этапов 0–5 (ваш сильный навык) — это даже желательно, чтобы к C6 API был готов.

Не начинайте C6 без:

- [ ] закрытого Part B
- [ ] работающего `GET /api/categories` на PostgreSQL

---

## Часть F — Карта «сессия → файл / фича»

| Сессия | Результат в `rewrite/client` | STACK |
|--------|------------------------------|-------|
| C6.1 | проект создан | этап 6 |
| C6.2 | `environments` | этап 6 |
| C6.3–C6.6 | categories feature | этап 6 DoD |
| C7.1 | routes + shell | этап 7 |
| C7.2–C7.3 | list/details | этап 7 |
| C7.4–C7.7 | auth pipeline | этап 7 |
| C7.8–C7.9 | create + UX | этап 7 DoD |
| C8A.* | answers на details | этап 8A |
| C8B.* | votes | этап 8B |
| C8C.* | admin approval | этап 8C |

Blazor-ориентиры:

| Angular экран | Blazor |
|---------------|--------|
| List | [`Index.razor`](../QuestionAppUI/Pages/Index.razor) |
| Details | [`Details.razor`](../QuestionAppUI/Pages/Details.razor) |
| Create | [`Create.razor`](../QuestionAppUI/Pages/Create.razor) |
| Login | [`Login.cshtml`](../QuestionAppUI/Areas/Identity/Pages/Account/Login.cshtml) |
| Admin | [`AdminApproval.razor`](../QuestionAppUI/Pages/AdminApproval.razor) |

---

## Часть G — Ресурсы (короткий список)

Официальное — приоритет над «случайным YouTube 2019»:

1. [angular.dev](https://angular.dev/) — Essentials, Tutorials  
2. [TypeScript Handbook](https://www.typescriptlang.org/docs/handbook/intro.html)  
3. [MDN Learn Web](https://developer.mozilla.org/en-US/docs/Learn) — HTML / CSS / JS  
4. RxJS: только [observables overview](https://angular.dev/guide/http) в контексте HttpClient на старте  

Для .NET-мозга полезный внутренний вопрос на каждой сессии:

> «Как бы я сделал это в Blazor / ASP.NET, и какое здесь отличие вынужденное (браузер), а какое — просто другой синтаксис?»

---

## Критерий «я влился в Angular достаточно для этого проекта»

Вы можете:

1. Объяснить дерево: Component → Service → HttpClient → Web API → EF → PostgreSQL.  
2. Собрать экран со списком, деталями, формой, loading/error.  
3. Провести JWT через interceptor и guard и понимать их пределы.  
4. Читать ошибки в Console/Network без паники.  
5. Отличать TypeScript-контракт от C# DTO и не ждать «общих классов» между проектами.  

Когда это так — Part C закрыта по смыслу; дальше наращивайте фичи (Profile, AdminPanel, upload) уже как .NET-разработчик, который **умеет** открыть Angular-код и продолжить.

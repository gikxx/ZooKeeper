# ZooKeeper - Система управления зоопарком

## Описание проекта
Система управления зоопарком (для управления животными, вольерами и процессами их обслуживания), разработанная с использованием принципов Domain-Driven Design и Clean Architecture. Проект реализован в качестве мини-домашнего задания №2 по предмету "Конструирование программного обеспечения". 

## Структура решения
Проект разделен на слои согласно Clean Architecture:

- **ZooKeeper.Domain**: Содержит бизнес-логику и правила
- **ZooKeeper.Application**: Содержит сценарии использования
- **ZooKeeper.Infrastructure**: Реализует персистентность данных (in-memory)
- **ZooKeeper.Presentation**: Контроллеры веб-приложения
- **ZooKeeper.Tests**: Модульные тесты

## Реализованная функциональность

### Доменная модель (DDD)
- Value Objects: `AnimalId`, `EnclosureId`, `AnimalName`,..
- Entities: `Animal`, `Enclosure`, `FeedingSchedule`
- Доменные события: `AnimalMovedEvent`, `FeedingTimeEvent`

### Endpoints
- `AnimalsController`:
  - GET `/api/animals` - получение списка животных
  - GET `/api/animals/{id}` - получение информации о животном
  - POST `/api/animals` - добавление нового животного
  - DELETE `/api/animals/{id}` - удаление животного
  - POST `/api/animals/{id}/transfer` - перемещение животного

- `EnclosuresController`:
  - GET `/api/enclosures` - получение списка вольеров
  - GET `/api/enclosures/{id}` - получение информации о вольере
  - POST `/api/enclosures` - создание вольера
  - GET `/api/enclosures/{id}/animals` - получение списка животных в вольере
  - DELETE `/api/enclosures/{id}` - удаление вольера

## Применение DDD и Clean Architecture

### Domain-Driven Design
1. Value Objects: 
   - Инкапсуляция бизнес-правил в `AnimalName`, `AnimalId`
   - Валидация в конструкторах

2. Entities:
   - Инкапсуляция состояния в `Animal`, `Enclosure`
   - Защита инвариантов через методы

3. Domain Events:
   - События при перемещении животных

### Clean Architecture
1. Зависимости направлены к центру (Domain), сам Domain ни от чего не зависит
2. Инверсия зависимостей через интерфейсы:
   - `IAnimalRepository`
   - `IEnclosureRepository`
   - `IFeedingScheduleRepository`
   - `IDomainEventPublisher`
   - `IRepository`

3. Изоляция бизнес-логики:
   - **ZooKeeper.Domain**: бизнес-правила и сущности
   - **ZooKeeper.Application**: сценарии использования

## Тестирование
- Модульные тесты для контроллеров
- Тесты доменной логики
- Тесты сценариев использования
- Покрытие кода: >65%

## Технологии
- .NET 9
- ASP.NET Core
- Swagger/OpenAPI
- xUnit
- FluentAssertions
- Moq

## Запуск проекта
1. Клонировать репозиторий
2. Открыть решение в Visual Studio/Rider
3. Запустить проект `ZooKeeper.Presentation`
4. Swagger UI доступен по адресу `/swagger`

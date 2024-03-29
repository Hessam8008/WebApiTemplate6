﻿using Application.Abstractions;

namespace Application.Persons.Commands.ChangeNation;

public record ChangeNationCommand(Guid Id, string Nation) : ICommand;
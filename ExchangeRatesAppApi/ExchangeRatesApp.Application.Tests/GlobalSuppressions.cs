﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S6966:Awaitable method should be used", Justification = "Unit test project does not require strict rules", Scope = "member", Target = "~M:ExchangeRatesApp.Application.Tests.Voting.AddCandidateCommandHandlerTests.ShouldThrowWhenUsernameExists(System.String)~System.Threading.Tasks.Task")]

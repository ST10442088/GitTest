// Copyright Epic Games, Inc. All Rights Reserved.

#include "BP_ConversionAttemptGameMode.h"
#include "BP_ConversionAttemptCharacter.h"
#include "UObject/ConstructorHelpers.h"

ABP_ConversionAttemptGameMode::ABP_ConversionAttemptGameMode()
	: Super()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPerson/Blueprints/BP_FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

}

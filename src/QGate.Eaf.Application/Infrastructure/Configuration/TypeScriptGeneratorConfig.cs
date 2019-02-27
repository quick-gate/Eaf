﻿using QGate.Core.Collections;
using QGate.Eaf.Domain.Components.Entities;
using QGate.Eaf.Domain.Entities.Models.Params;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QGate.Eaf.Application.Infrastructure.Configuration
{
    public static class TypeScriptGeneratorConfig
    {
        private const string _domainModelsNamespace = "QGate.Eaf.Domain.Models.";
        private const string _typeScriptModelPostfix = ".model.ts";
        private const string _typeScriptEnumPostfix = ".enum.ts";

        public static void Configure(ConfigurationBuilder builder)
        {
            builder.Substitute(typeof(DateTime), new RtSimpleTypeName("Date"));
            builder.Substitute(typeof(Guid), new RtSimpleTypeName("string"));
            builder.Global(c =>
            {
                c.UseModules(true);
            });

            //ConfigureAssembly(builder, typeof(EntityList).Assembly, _domainModelsNamespace);
            ConfigureInternal(builder, new List<Type>
            {
                typeof(EntityList),
                typeof(EntityListAttribute),
                typeof(GetEntityListParams),
                typeof(GetEntityListResult)
            }, null, _domainModelsNamespace);
        }

        private static void ConfigureAssembly(ConfigurationBuilder builder, Assembly assembly, string modelsBaseNamespace)
        {
            var classes = new List<Type>();
            var enums = new List<Type>();

            foreach (var type in assembly.GetTypes().Where(x => !string.IsNullOrEmpty(x.Namespace) && x.Namespace.StartsWith(modelsBaseNamespace)))
            {
                if (type.IsClass)
                {
                    classes.Add(type);
                }
                else if (type.IsEnum)
                {
                    enums.Add(type);
                }
            }

            ConfigureInternal(builder, classes, enums, modelsBaseNamespace);
        }

        private static void ConfigureInternal(ConfigurationBuilder builder, IList<Type> classes, IList<Type> enums, string modelsNamespace)
        {
            if (!classes.IsNullOrEmpty())
            {
                builder.ExportAsClasses(classes, x =>
                {
                    x.WithPublicProperties();
                    x.ExportTo(GetTypeScriptPath(x.Type, modelsNamespace));
                });
            }

            if (!enums.IsNullOrEmpty())
            {
                builder.ExportAsEnums(enums, x =>
                {
                    x.ExportTo(GetTypeScriptPath(x.Type, modelsNamespace));
                });
            }
        }

        private static string GetTypeScriptPath(Type type, string modelsNamespace) => string.Concat(type.Namespace.Replace(modelsNamespace, string.Empty).Replace(".", "/"), "/", type.Name,
                type.IsEnum ? _typeScriptEnumPostfix : _typeScriptModelPostfix);
    }
}
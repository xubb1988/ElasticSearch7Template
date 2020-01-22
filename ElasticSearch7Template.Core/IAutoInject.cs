using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Core
{
    /// <summary>
    /// 自动注入扫描点默认为Scoped
    /// </summary>
    public interface IAutoInject
    {
    }

    /// <summary>
    ///  自动注入接口和实现Scoped类型
    /// </summary>
    public interface IScopedAutoInject
    {

    }

    /// <summary>
    /// 自动注入接口和实现Singleton类型
    /// </summary>
    public interface ISingletonAutoInject
    {

    }

    /// <summary>
    /// 自动注入接口和实现Transient类型
    /// </summary>
    public interface ITransientAutoInject
    {

    }

    /// <summary>
    /// 自动注入自身Scoped类型
    /// </summary>
    public interface ISelfScopedAutoInject
    {

    }

    /// <summary>
    /// 自动注入自身Singleton类型
    /// </summary>
    public interface ISelfSingletonAutoInject
    {

    }

    /// <summary>
    /// 自动注入自身Transient类型
    /// </summary>
    public interface ISelfTransientAutoInject
    {

    }
}
